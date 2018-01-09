using System;
using System.Security.Permissions;

namespace Chenyuan.Caching.Defaults
{
    internal class SRefMultiple
    {
  //      [CompilerGenerated]
  //      [Serializable]
  //      private sealed class <>c
		//{
		//	public static readonly SRefMultiple.<>c<>9 = new SRefMultiple.<>c();
  //      public static Func<SRef, long> <>9__3_0;
		//	internal long <get_ApproximateSize>b__3_0(SRef s)
  //      {
  //          return s.ApproximateSize;
  //      }
  //  }
    //private List<SRef> _srefs = new List<SRef>();
    internal long ApproximateSize
    {
        [PermissionSet(SecurityAction.Assert, Unrestricted = true)]
        get
        {
                throw new NotImplementedException();
                //        IEnumerable<SRef> arg_25_0 = _srefs;
                //        Func<SRef, long> arg_25_1;
                //        if ((arg_25_1 = SRefMultiple.<> c.<> 9__3_0) == null)
                //{
                //            arg_25_1 = (SRefMultiple.<> c.<> 9__3_0 = new Func<SRef, long>(SRefMultiple.<> c.<> 9.< get_ApproximateSize > b__3_0));
                //        }
                //        return arg_25_0.Sum(arg_25_1);
            }
        }
    internal void AddSRefTarget(object o)
    {
        //_srefs.Add(new SRef(o));
    }
    [PermissionSet(SecurityAction.Assert, Unrestricted = true)]
    internal void Dispose()
    {
            throw new NotImplementedException();
        //using (List<SRef>.Enumerator enumerator = _srefs.GetEnumerator())
        //{
        //    while (enumerator.MoveNext())
        //    {
        //        enumerator.Current.Dispose();
        //    }
        //}
    }
}}