## Cache Decorator 範例專案


此範例只有提供 .NET Core 版本 (ASP.NET Core 2.2)

說明如何使用 Decorator Pattern (裝飾者模式) 實做多層快取處理，這邊就不多解釋說明什麼是 Decorator Pattern (裝飾者模式) ，想要了解的人可以自己去查詢或是看書（深入淺出設計模式、大話設計模式等等）。


在以往的專案開發裡，程式如果要加上快取處理，最直接也是最快的作法就是在程式裡添加快取處理的程式，但這樣的作法卻是最不好維護與最容易出問題的，因為快取處理散落在專案的各個程式裡，如果快取處理要更換別種作法，就需要將專案裡的所有程式都翻出來然後逐一修改，但總是會有遺漏或是寫錯的時候，所以出問題的機會就很高。另外就是如果一種快取處理不夠用，而要增加第二種甚至第三種的快取處理時，這時候又開始再把程式都翻出來然後加程式，越多的程式、越多的疊床架屋就等於產生更多的問題與錯誤。

假如今天我要觀測在沒有快取處理時的程式執行狀況時，如果是使用以上的直接做法時會怎麼做呢？是不是要把執行時會經過程式的所有快取都給拔掉（或用註解方式），等到該處理的、該拔的、該隱藏的都做完後才能執行呢?

### 開始說明 ###

使用裝飾者模式來實做快取處理，一開始的優先考量就是不要因為要添加功能而破壞原有程式的邏輯。以資料存取的程式來做說明，以下是範例專案裡的 FooRepository - GetAsync 方法

```
    public async Task<FooModel> GetAsync(Guid id)
    {
        var stepName = $"{nameof(FooRepository)}.GetAsync";
        using (ProfilingSession.Current.Step(stepName))
        {
            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentNullException(nameof(id));
            }

            using (var conn = this.DatabaseHelper.GetConnection())
            {
                var sqlCommand = new StringBuilder();
                sqlCommand.AppendLine("SELECT [FooId] ");
                sqlCommand.AppendLine("      ,[Name] ");
                sqlCommand.AppendLine("      ,[Description] ");
                sqlCommand.AppendLine("      ,[Enable] ");
                sqlCommand.AppendLine("      ,[CreateTime] ");
                sqlCommand.AppendLine("      ,[UpdateTime] ");
                sqlCommand.AppendLine("  FROM [dbo].[Foo] ");
                sqlCommand.AppendLine("  Where ");
                sqlCommand.AppendLine("    FooId = @FooId ;");

                var parameters = new DynamicParameters();
                parameters.Add("FooId", id);

                var query = await this.DapperHelper.QueryFirstOrDefaultAsync<FooModel>
                (
                    dbConnection: conn,
                    sql: sqlCommand.ToString(),
                    param: parameters
                );

                return query;
            }
        }
    }
```

我盡可能不去破壞這裡的程式邏輯，讓程式保持最基本的樣貌，而要加上記憶體快取處理就會在另一個 CachedFooRepository - GetAsync 去添加功能

```
    public async Task<FooModel> GetAsync(Guid id)
    {
        var stepName = $"{nameof(CachedFooRepository)}.GetAsync";
        using (ProfilingSession.Current.Step(stepName))
        {
            var cacheItem = await this.GetOrAddCacheItemAsync
            (
                Cachekeys.Foo.Get.ToFormat(id),
                CacheUtility.GetCacheItemExpiration5Min(),
                async () =>
                {
                    var result = await this.FooRepository.GetAsync(id);
                    return result;
                }
            );

            return cacheItem;
        }
    }

```

只要在 DI 設定時使用 Scrutor 的 Decorate<TService, TDecorator>() 方法就可以優雅的處理裝飾者註冊

```
    services.AddScoped<IFooRepository, FooRepository>()
            .Decorate<IFooRepository, CachedFooRepository>();
```

如果要再加上 Redis 快取，只需要增加 RedisFooReposotory 類別和完成實做內容，最後 DI 的裝飾者註冊加上就可以

```
    services.AddScoped<IFooRepository, FooRepository>()
            .Decorate<IFooRepository, RedisFooRepository>()
            .Decorate<IFooRepository, CachedFooRepository>();
```

### 開放封閉原則 ###

不對原本的 FooRepository 類別裡的程式做任何的修改與更動，只有增加新的裝飾者類別和記得在 DI 去註冊裝飾者類別就可以了，這就能夠達到物件導向程式設計 SOLID 原則的「開放封閉原則 OCP (The Open-Closed Pattern)」，讓變動與修改的地方越少越好（改越多、錯誤就會越多）

```
開放封閉原則 (The Open-Closed Pattern)
對擴充開放、對修改關閉
```

### 快取裝飾者設定檔 ###

這個設定檔的內容與值的順序與裝飾者的 DI 註冊設定無關，不這麼做是別讓 DI 註冊複雜下去

設定檔內容可以設定專案會用到的 CacheProvider，並且設定各個快取裝飾類別的定義(介面)和實做

```
{
  "CacheDecoratorSettings": {
    "CacheProviders": [ "MemoryCacheProvider", "RedisCacheProvider" ],
    "CacheDecorators": [
      {
        "Declaration": "IFooRepository",
        "Implements": [ "FooRepository", "RedisFooRepository", "CachedFooRepository" ]
      }
    ]
  }
}
```

如果今天要拔掉其中一個快取裝飾者，則需要到 DI 設定裡移除掉裝飾者註冊，或者是修改設定檔的 CacheProviders 內容，移除快取裝飾者所依賴的 CacheProvider，當依賴的 CacheProvider 不在設定清單裡，就會使用預設的 NullCacheProvider，這個 NullCacheProvider 的內容就是什麼快取處理都沒有、也不做什麼處理，直接存取被裝飾者的方法。

如下面的設定，將 RedisCacheProvider 從 CacheProviders 裡移除，這麼一來 RedisFooRepository 要依賴使用的快取提供者就會改為使用預設的 NullCacheProvider

```
{
  "CacheDecoratorSettings": {
    "CacheProviders": [ "MemoryCacheProvider" ],
    "CacheDecorators": [
      {
        "Declaration": "IFooRepository",
        "Implements": [ "FooRepository", "RedisFooRepository", "CachedFooRepository" ]
      }
    ]
  }
}
```

另外也可以修改快取裝飾者的 Implements 內容，將某個裝飾者類別移除，這麼一來該裝飾者就會使用 NullCacheProvider，下面的設定裡已經把 RedisFooRepository 給移除，程式實際執行時還是會進入 RedisFooRepository 但所依賴的 CacheProvider 將會使用 NullCacheProvider

```
{
  "CacheDecoratorSettings": {
    "CacheProviders": [ "MemoryCacheProvider", "RedisCacheProvider" ],
    "CacheDecorators": [
      {
        "Declaration": "IFooRepository",
        "Implements": [ "FooRepository", "CachedFooRepository" ]
      }
    ]
  }
}
```

### 套件使用 ###

Scrutor https://github.com/khellang/Scrutor

快取
```
Microsoft.Extensions.Caching.Abstractions
Microsoft.Extensions.Caching.Memory
Microsoft.Extensions.Caching.StackExchangeRedis (專案須使用 .NET Core 2.2)
```

MessagePack, MessagePackAnalyzer

使用 MessagePack 處理快取資料，將資料序列化為 Binary 以放進 Redis，這邊當然也可以使用其他的 BinaryFormatter 套件，使用 MessagePack 的原因在於處理速度與序列化之後的資料大小，另外就是專案使用後的變化修改成本較小

https://github.com/neuecc/MessagePack-CSharp

快速序列化组件MessagePack介绍 - 晓晨Master - 博客园 https://www.cnblogs.com/stulzq/p/8039933.html


Serialization Performance Update With .NET 4.7.2 - Alois Kraus https://aloiskraus.wordpress.com/2018/05/06/serialization-performance-update-with-net-4-7-2/


### Redis 設定 ###

Startup.cs - ConfigureServices 方法

```
    // 快取裝飾者設定
    var cacheDecoratorSettings = new CacheDecoratorSettings();
    this.Configuration.GetSection("CacheDecoratorSettings").Bind(cacheDecoratorSettings);
    this.CacheDecoratorSettings = cacheDecoratorSettings;

    services.AddMemoryCache();

    services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = "127.0.0.1:6379";
        options.InstanceName = "CacheDecorator_Sample";
    });
```
上面的 servicesAddStackExchangeRedisCache() 設定，Configuration 為 Redis 的 IP 或服務網址與 Port，如果要用在公司的正式專案就要改用 Evertrust.Settings.Url 套件來取得 Redis 的服務網址。

InstanceName 為專案的產品名稱，這邊要注意一下放進 Redis 的資料所使用的有效快取鍵格式

```
{ProductName}::{CacheName}::{Key}
```
ProductName 就是 options.InstanceName，CacheName 為快取類別名稱，Key 為快取名稱且為唯一值，所以每一筆快取資料的 Key 都會不同。

在範例裡的 Redis 快取資料存續時間都是使用 AbsoluteExpirationRelativeToNow，資料的存續時間為固定的 (可以自行決定時間的長度) 不會做延展。

### 資料快取要注意的事情 ###

在專案裡最常使用的記憶體快取，但是記憶體快取會佔用系統資源，所以不適合大量的快取資料做長時間的暫留，而且一旦網站系統重啟或是伺服器重開機的情況時，記憶體快取就會消失並且要重新向來源端取得並重新製作快取。

記憶體快取的存續時間要短，依據資料的性質與異動頻繁度來做決定，不宜將存續時間設定太長，通常我預設都是設定為五分鐘，最長為一小時。

另外記憶體快取處理通常是取得資料的第一到關卡，所以要做好鎖定處理以避免瞬時過多 Request 湧入而直接衝到資料庫索而讓資料庫負載過重的情況，所以這邊可以參考黑暗執行緒所分享的一篇文章來製作同步鎖

改良式GetCachableData可快取查詢函式-黑暗執行緒 https://blog.darkthread.net/blog/improved-getcachabledata/

要放入 Redis 的快取資料其存續時間可以比記憶體快取長，我基本上都是設定一個小時，不過還是要看資料的性質與異動的頻繁度來做決定，另外 Redis 快取不適合單一快取資料裡放進大量的 Collection 內容，畢竟還是要經過內部網路的傳輸以及序列化、反序列化的處理，所以是否要在單一快取資料裡放入大量的 Collection 內容就必須要好好思考。

至於快取資料因為原始資料來源的異動而需要做更新，這部份的處理可以參考之前分享過的內容做整合應用
