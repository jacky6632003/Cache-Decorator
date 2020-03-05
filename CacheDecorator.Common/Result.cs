using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CacheDecorator.Common
{
    [Serializable]
    public class Result : IResult
    {
        protected static object Lock;

        private bool _success;

        private string _message;

        public int AffectRows
        {
            get;
            set;
        }

        public Exception Exception
        {
            get;
            set;
        }

        public Guid ID
        {
            get;
            set;
        }

        public List<IResult> InnerResults
        {
            get
            {
                return JustDecompileGenerated_get_InnerResults();
            }
            set
            {
                JustDecompileGenerated_set_InnerResults(value);
            }
        }

        private List<IResult> JustDecompileGenerated_InnerResults_k__BackingField;

        public List<IResult> JustDecompileGenerated_get_InnerResults()
        {
            return this.JustDecompileGenerated_InnerResults_k__BackingField;
        }

        protected void JustDecompileGenerated_set_InnerResults(List<IResult> value)
        {
            this.JustDecompileGenerated_InnerResults_k__BackingField = value;
        }

        public string Message
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append(this._message);
                foreach (Result innerResult in this.InnerResults)
                {
                    if (innerResult.Success || String.IsNullOrEmpty(innerResult.Message))
                    {
                        continue;
                    }
                    stringBuilder.AppendLine();
                    stringBuilder.Append(innerResult.Message);
                }
                return stringBuilder.ToString();
            }
            set
            {
                this._message = value;
            }
        }

        public bool Success
        {
            get
            {
                bool flag;
                lock (Result.Lock)
                {
                    List<IResult>.Enumerator enumerator = this.InnerResults.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            Result current = (Result)enumerator.Current;
                            if (current == null || current.Success)
                            {
                                continue;
                            }
                            flag = false;
                            return flag;
                        }
                        return this._success;
                    }
                    finally
                    {
                        ((IDisposable)enumerator).Dispose();
                    }
                }
                return flag;
            }
            set
            {
                this._success = value;
            }
        }

        static Result()
        {
            Result.Lock = new Object();
        }

        public Result() : this(false)
        {
        }

        public Result(bool success)
        {
            this.ID = Guid.NewGuid();
            this.Success = success;
            this.InnerResults = new List<IResult>();
        }

        public IResult AddResult(IResult innerResult)
        {
            this.InnerResults.Add(innerResult);
            return this;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override string ToString()
        {
            if (!this.Success)
            {
                return "Failure";
            }
            return "Success";
        }
    }

    [Serializable]
    public class Result<T> : IResult<T>
    {
        protected static object Lock;

        private bool _success;

        public int AffectRows
        {
            get;
            set;
        }

        public Exception Exception
        {
            get;
            set;
        }

        public Guid ID
        {
            get;
            set;
        }

        public List<IResult<T>> InnerResults
        {
            get
            {
                return JustDecompileGenerated_get_InnerResults();
            }
            set
            {
                JustDecompileGenerated_set_InnerResults(value);
            }
        }

        private List<IResult<T>> JustDecompileGenerated_InnerResults_k__BackingField;

        public List<IResult<T>> JustDecompileGenerated_get_InnerResults()
        {
            return this.JustDecompileGenerated_InnerResults_k__BackingField;
        }

        protected void JustDecompileGenerated_set_InnerResults(List<IResult<T>> value)
        {
            this.JustDecompileGenerated_InnerResults_k__BackingField = value;
        }

        public string Message
        {
            get;
            set;
        }

        public T ReturnValue
        {
            get;
            set;
        }

        public bool Success
        {
            get
            {
                bool flag;
                lock (Result<T>.Lock)
                {
                    List<IResult<T>>.Enumerator enumerator = this.InnerResults.GetEnumerator();
                    try
                    {
                        while (enumerator.MoveNext())
                        {
                            Result<T> current = (Result<T>)enumerator.Current;
                            if (current.EqualNull() || current.Success)
                            {
                                continue;
                            }
                            flag = false;
                            return flag;
                        }
                        return this._success;
                    }
                    finally
                    {
                        ((IDisposable)enumerator).Dispose();
                    }
                }
                return flag;
            }
            set
            {
                this._success = value;
            }
        }

        static Result()
        {
            Result<T>.Lock = new Object();
        }

        public Result() : this(false)
        {
        }

        public Result(bool success)
        {
            this.ID = Guid.NewGuid();
            this.Success = success;
            this.InnerResults = new List<IResult<T>>();
        }

        public IResult<T> AddResult(IResult<T> innerResult)
        {
            this.InnerResults.Add(innerResult);
            return this;
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        public override string ToString()
        {
            if (!this.Success)
            {
                return "Failure";
            }
            return "Success";
        }
    }
}