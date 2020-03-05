using System;
using System.Collections.Generic;
using System.Text;

namespace CacheDecorator.Common
{
    public interface IResult
    {
        int AffectRows
        {
            get;
            set;
        }

        Exception Exception
        {
            get;
            set;
        }

        Guid ID
        {
            get;
            set;
        }

        List<IResult> InnerResults
        {
            get;
        }

        string Message
        {
            get;
            set;
        }

        bool Success
        {
            get;
            set;
        }

        IResult AddResult(IResult innerResult);

        string Serialize();
    }

    public interface IResult<T>
    {
        int AffectRows
        {
            get;
            set;
        }

        Exception Exception
        {
            get;
            set;
        }

        Guid ID
        {
            get;
            set;
        }

        List<IResult<T>> InnerResults
        {
            get;
        }

        string Message
        {
            get;
            set;
        }

        T ReturnValue
        {
            get;
            set;
        }

        bool Success
        {
            get;
            set;
        }

        IResult<T> AddResult(IResult<T> innerResult);

        string Serialize();
    }
}