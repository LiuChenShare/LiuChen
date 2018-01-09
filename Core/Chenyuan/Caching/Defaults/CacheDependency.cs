using System;
using System.Text;

namespace Chenyuan.Caching.Defaults
{
    public class CacheDependency : IDisposable
    {
        internal class DepFileInfo
        {
            internal string _filename;
            //internal FileAttributesData _fad;
        }
        private string _uniqueID;
        private object _depFileInfos;
        private object _entries;
        private ICacheDependencyChanged _objNotify;
        //private SafeBitVector32 _bits;
        private DateTime _utcLastModified;
        private static readonly string[] s_stringsEmpty;
        private static readonly CacheEntry[] s_entriesEmpty;
        private static readonly CacheDependency s_dependencyEmpty;
        private static readonly CacheDependency.DepFileInfo[] s_depFileInfosEmpty;
        private static readonly TimeSpan FUTURE_FILETIME_BUFFER;
        private const int BASE_INIT = 1;
        private const int USED = 2;
        private const int CHANGED = 4;
        private const int BASE_DISPOSED = 8;
        private const int WANTS_DISPOSE = 16;
        private const int DERIVED_INIT = 32;
        private const int DERIVED_DISPOSED = 64;
        public bool HasChanged
        {
            get
            {
                throw new NotImplementedException();
                //return _bits[4];
            }
        }
        public DateTime UtcLastModified
        {
            get
            {
                return _utcLastModified;
            }
        }
        internal CacheEntry[] CacheEntries
        {
            get
            {
                if (_entries == null)
                {
                    return null;
                }
                CacheEntry cacheEntry = _entries as CacheEntry;
                if (cacheEntry != null)
                {
                    return new CacheEntry[]
                    {
                        cacheEntry
                    };
                }
                return (CacheEntry[])_entries;
            }
        }
        static CacheDependency()
        {
            CacheDependency.FUTURE_FILETIME_BUFFER = new TimeSpan(0, 1, 0);
            CacheDependency.s_stringsEmpty = new string[0];
            CacheDependency.s_entriesEmpty = new CacheEntry[0];
            CacheDependency.s_dependencyEmpty = new CacheDependency(0);
            CacheDependency.s_depFileInfosEmpty = new CacheDependency.DepFileInfo[0];
        }
        private CacheDependency(int bogus)
        {
        }
        protected CacheDependency()
        {
            this.Init(true, null, null, null, DateTime.MaxValue);
        }
        public CacheDependency(string filename) : this(filename, DateTime.MaxValue)
        {
        }
        public CacheDependency(string filename, DateTime start)
        {
            throw new NotImplementedException();
            //if (filename == null)
            //{
            //    return;
            //}
            //DateTime utcStart = DateTimeUtil.ConvertToUniversalTime(start);
            //string[] filenamesArg = new string[]
            //{
            //    filename
            //};
            //this.Init(true, filenamesArg, null, null, utcStart);
        }
        public CacheDependency(string[] filenames)
        {
            this.Init(true, filenames, null, null, DateTime.MaxValue);
        }
        public CacheDependency(string[] filenames, DateTime start)
        {
            throw new NotImplementedException();
            //DateTime utcStart = DateTimeUtil.ConvertToUniversalTime(start);
            //this.Init(true, filenames, null, null, utcStart);
        }
        public CacheDependency(string[] filenames, string[] cachekeys)
        {
            this.Init(true, filenames, cachekeys, null, DateTime.MaxValue);
        }
        public CacheDependency(string[] filenames, string[] cachekeys, DateTime start)
        {
            throw new NotImplementedException();
            //DateTime utcStart = DateTimeUtil.ConvertToUniversalTime(start);
            //this.Init(true, filenames, cachekeys, null, utcStart);
        }
        public CacheDependency(string[] filenames, string[] cachekeys, CacheDependency dependency)
        {
            this.Init(true, filenames, cachekeys, dependency, DateTime.MaxValue);
        }
        public CacheDependency(string[] filenames, string[] cachekeys, CacheDependency dependency, DateTime start)
        {
            throw new NotImplementedException();
            //DateTime utcStart = DateTimeUtil.ConvertToUniversalTime(start);
            //this.Init(true, filenames, cachekeys, dependency, utcStart);
        }
        internal CacheDependency(int dummy, string filename) : this(dummy, filename, DateTime.MaxValue)
        {
        }
        internal CacheDependency(int dummy, string filename, DateTime utcStart)
        {
            if (filename == null)
            {
                return;
            }
            string[] filenamesArg = new string[]
            {
                filename
            };
            this.Init(false, filenamesArg, null, null, utcStart);
        }
        internal CacheDependency(int dummy, string[] filenames)
        {
            this.Init(false, filenames, null, null, DateTime.MaxValue);
        }
        internal CacheDependency(int dummy, string[] filenames, DateTime utcStart)
        {
            this.Init(false, filenames, null, null, utcStart);
        }
        internal CacheDependency(int dummy, string[] filenames, string[] cachekeys)
        {
            this.Init(false, filenames, cachekeys, null, DateTime.MaxValue);
        }
        internal CacheDependency(int dummy, string[] filenames, string[] cachekeys, DateTime utcStart)
        {
            this.Init(false, filenames, cachekeys, null, utcStart);
        }
        internal CacheDependency(int dummy, string[] filenames, string[] cachekeys, CacheDependency dependency)
        {
            this.Init(false, filenames, cachekeys, dependency, DateTime.MaxValue);
        }
        internal CacheDependency(int dummy, string[] filenames, string[] cachekeys, CacheDependency dependency, DateTime utcStart)
        {
            this.Init(false, filenames, cachekeys, dependency, utcStart);
        }
        private void Init(bool isPublic, string[] filenamesArg, string[] cachekeysArg, CacheDependency dependency, DateTime utcStart)
        {
            throw new NotImplementedException();
            //CacheDependency.DepFileInfo[] array = CacheDependency.s_depFileInfosEmpty;
            //CacheEntry[] array2 = CacheDependency.s_entriesEmpty;
            //_bits = new SafeBitVector32(0);
            //string[] array3;
            //if (filenamesArg != null)
            //{
            //    array3 = (string[])filenamesArg.Clone();
            //}
            //else
            //{
            //    array3 = null;
            //}
            //string[] array4;
            //if (cachekeysArg != null)
            //{
            //    array4 = (string[])cachekeysArg.Clone();
            //}
            //else
            //{
            //    array4 = null;
            //}
            //_utcLastModified = DateTime.MinValue;
            //try
            //{
            //    if (array3 == null)
            //    {
            //        array3 = CacheDependency.s_stringsEmpty;
            //    }
            //    else
            //    {
            //        string[] array5 = array3;
            //        for (int i = 0; i < array5.Length; i++)
            //        {
            //            string text = array5[i];
            //            if (text == null)
            //            {
            //                throw new ArgumentNullException("filenamesArg");
            //            }
            //            if (isPublic)
            //            {
            //                InternalSecurityPermissions.PathDiscovery(text).Demand();
            //            }
            //        }
            //    }
            //    if (array4 == null)
            //    {
            //        array4 = CacheDependency.s_stringsEmpty;
            //    }
            //    else
            //    {
            //        string[] array5 = array4;
            //        for (int i = 0; i < array5.Length; i++)
            //        {
            //            if (array5[i] == null)
            //            {
            //                throw new ArgumentNullException("cachekeysArg");
            //            }
            //        }
            //    }
            //    if (dependency == null)
            //    {
            //        dependency = CacheDependency.s_dependencyEmpty;
            //    }
            //    else
            //    {
            //        if (dependency.GetType() != CacheDependency.s_dependencyEmpty.GetType())
            //        {
            //            throw new ArgumentException(SR.GetString("Invalid_Dependency_Type"));
            //        }
            //        object depFileInfos = dependency._depFileInfos;
            //        object entries = dependency._entries;
            //        DateTime utcLastModified = dependency._utcLastModified;
            //        if (dependency._bits[4])
            //        {
            //            _bits[4] = true;
            //            this.DisposeInternal();
            //            return;
            //        }
            //        if (depFileInfos != null)
            //        {
            //            if (depFileInfos is CacheDependency.DepFileInfo)
            //            {
            //                array = new CacheDependency.DepFileInfo[]
            //                {
            //                    (CacheDependency.DepFileInfo)depFileInfos
            //                };
            //            }
            //            else
            //            {
            //                array = (CacheDependency.DepFileInfo[])depFileInfos;
            //            }
            //            CacheDependency.DepFileInfo[] array6 = array;
            //            for (int i = 0; i < array6.Length; i++)
            //            {
            //                string filename = array6[i]._filename;
            //                if (filename == null)
            //                {
            //                    _bits[4] = true;
            //                    this.DisposeInternal();
            //                    return;
            //                }
            //                if (isPublic)
            //                {
            //                    InternalSecurityPermissions.PathDiscovery(filename).Demand();
            //                }
            //            }
            //        }
            //        if (entries != null)
            //        {
            //            if (entries is CacheEntry)
            //            {
            //                array2 = new CacheEntry[]
            //                {
            //                    (CacheEntry)entries
            //                };
            //            }
            //            else
            //            {
            //                array2 = (CacheEntry[])entries;
            //                CacheEntry[] array7 = array2;
            //                for (int i = 0; i < array7.Length; i++)
            //                {
            //                    if (array7[i] == null)
            //                    {
            //                        _bits[4] = true;
            //                        this.DisposeInternal();
            //                        return;
            //                    }
            //                }
            //            }
            //        }
            //        _utcLastModified = utcLastModified;
            //    }
            //    int num = array.Length + array3.Length;
            //    if (num > 0)
            //    {
            //        CacheDependency.DepFileInfo[] array8 = new CacheDependency.DepFileInfo[num];
            //        FileChangeEventHandler callback = new FileChangeEventHandler(this.FileChange);
            //        FileChangesMonitor fileChangesMonitor = HttpRuntime.FileChangesMonitor;
            //        int j;
            //        for (j = 0; j < num; j++)
            //        {
            //            array8[j] = new CacheDependency.DepFileInfo();
            //        }
            //        j = 0;
            //        CacheDependency.DepFileInfo[] array6 = array;
            //        for (int i = 0; i < array6.Length; i++)
            //        {
            //            string filename2 = array6[i]._filename;
            //            fileChangesMonitor.StartMonitoringPath(filename2, callback, out array8[j]._fad);
            //            array8[j]._filename = filename2;
            //            j++;
            //        }
            //        DateTime dateTime = DateTime.MinValue;
            //        string[] array5 = array3;
            //        for (int i = 0; i < array5.Length; i++)
            //        {
            //            string text2 = array5[i];
            //            DateTime dateTime2 = fileChangesMonitor.StartMonitoringPath(text2, callback, out array8[j]._fad);
            //            array8[j]._filename = text2;
            //            j++;
            //            if (dateTime2 > _utcLastModified)
            //            {
            //                _utcLastModified = dateTime2;
            //            }
            //            if (utcStart < DateTime.MaxValue)
            //            {
            //                if (dateTime == DateTime.MinValue)
            //                {
            //                    dateTime = DateTime.UtcNow;
            //                }
            //                if (dateTime2 >= utcStart && !(dateTime2 - dateTime > CacheDependency.FUTURE_FILETIME_BUFFER))
            //                {
            //                    _bits[4] = true;
            //                    break;
            //                }
            //            }
            //        }
            //        if (array8.Length == 1)
            //        {
            //            _depFileInfos = array8[0];
            //        }
            //        else
            //        {
            //            _depFileInfos = array8;
            //        }
            //    }
            //    int num2 = array2.Length + array4.Length;
            //    if (num2 > 0 && !_bits[4])
            //    {
            //        CacheEntry[] array9 = new CacheEntry[num2];
            //        int num3 = 0;
            //        CacheEntry[] array7 = array2;
            //        for (int i = 0; i < array7.Length; i++)
            //        {
            //            CacheEntry cacheEntry = array7[i];
            //            cacheEntry.AddCacheDependencyNotify(this);
            //            array9[num3++] = cacheEntry;
            //        }
            //        CacheInternal cacheInternal = HttpRuntime.CacheInternal;
            //        string[] array5 = array4;
            //        for (int i = 0; i < array5.Length; i++)
            //        {
            //            string key = array5[i];
            //            CacheEntry cacheEntry2 = (CacheEntry)cacheInternal.DoGet(isPublic, key, CacheGetOptions.ReturnCacheEntry);
            //            if (cacheEntry2 == null)
            //            {
            //                _bits[4] = true;
            //                break;
            //            }
            //            cacheEntry2.AddCacheDependencyNotify(this);
            //            array9[num3++] = cacheEntry2;
            //            if (cacheEntry2.UtcCreated > _utcLastModified)
            //            {
            //                _utcLastModified = cacheEntry2.UtcCreated;
            //            }
            //            if (cacheEntry2.State != CacheEntry.EntryState.AddedToCache || cacheEntry2.UtcCreated > utcStart)
            //            {
            //                _bits[4] = true;
            //                break;
            //            }
            //        }
            //        if (array9.Length == 1)
            //        {
            //            _entries = array9[0];
            //        }
            //        else
            //        {
            //            _entries = array9;
            //        }
            //    }
            //    _bits[1] = true;
            //    if (dependency._bits[4])
            //    {
            //        _bits[4] = true;
            //    }
            //    if (_bits[16] || _bits[4])
            //    {
            //        this.DisposeInternal();
            //    }
            //}
            //catch
            //{
            //    _bits[1] = true;
            //    this.DisposeInternal();
            //    throw;
            //}
            //finally
            //{
            //    this.InitUniqueID();
            //}
        }
        public void Dispose()
        {
            throw new NotImplementedException();
            //_bits[32] = true;
            //if (this.Use())
            //{
            //    this.DisposeInternal();
            //}
        }
        protected internal void FinishInit()
        {
            throw new NotImplementedException();
            //_bits[32] = true;
            //if (_bits[16])
            //{
            //    this.DisposeInternal();
            //}
        }
        internal void DisposeInternal()
        {
            throw new NotImplementedException();
            //_bits[16] = true;
            //if (_bits[32] && _bits.ChangeValue(64, true))
            //{
            //    this.DependencyDispose();
            //}
            //if (_bits[1] && _bits.ChangeValue(8, true))
            //{
            //    this.DisposeOurself();
            //}
        }
        protected virtual void DependencyDispose()
        {
        }
        private void DisposeOurself()
        {
            throw new NotImplementedException();
            //object depFileInfos = _depFileInfos;
            //object entries = _entries;
            //_objNotify = null;
            //_depFileInfos = null;
            //_entries = null;
            //if (depFileInfos != null)
            //{
            //    FileChangesMonitor fileChangesMonitor = HttpRuntime.FileChangesMonitor;
            //    CacheDependency.DepFileInfo depFileInfo = depFileInfos as CacheDependency.DepFileInfo;
            //    if (depFileInfo != null)
            //    {
            //        fileChangesMonitor.StopMonitoringPath(depFileInfo._filename, this);
            //    }
            //    else
            //    {
            //        CacheDependency.DepFileInfo[] array = (CacheDependency.DepFileInfo[])depFileInfos;
            //        for (int i = 0; i < array.Length; i++)
            //        {
            //            string filename = array[i]._filename;
            //            if (filename != null)
            //            {
            //                fileChangesMonitor.StopMonitoringPath(filename, this);
            //            }
            //        }
            //    }
            //}
            //if (entries != null)
            //{
            //    CacheEntry cacheEntry = entries as CacheEntry;
            //    if (cacheEntry != null)
            //    {
            //        cacheEntry.RemoveCacheDependencyNotify(this);
            //        return;
            //    }
            //    CacheEntry[] array2 = (CacheEntry[])entries;
            //    for (int i = 0; i < array2.Length; i++)
            //    {
            //        CacheEntry cacheEntry2 = array2[i];
            //        if (cacheEntry2 != null)
            //        {
            //            cacheEntry2.RemoveCacheDependencyNotify(this);
            //        }
            //    }
            //}
        }
        internal bool Use()
        {
            throw new NotImplementedException();
            //return _bits.ChangeValue(2, true);
        }
        protected void SetUtcLastModified(DateTime utcLastModified)
        {
            _utcLastModified = utcLastModified;
        }
        internal void SetCacheDependencyChanged(ICacheDependencyChanged objNotify)
        {
            throw new NotImplementedException();
            //_bits[32] = true;
            //if (!_bits[8])
            //{
            //    _objNotify = objNotify;
            //}
        }
        internal void AppendFileUniqueId(CacheDependency.DepFileInfo depFileInfo, StringBuilder sb)
        {
            throw new NotImplementedException();
            //FileAttributesData fileAttributesData = depFileInfo._fad;
            //if (fileAttributesData == null)
            //{
            //    fileAttributesData = FileAttributesData.NonExistantAttributesData;
            //}
            //sb.Append(depFileInfo._filename);
            //sb.Append(fileAttributesData.UtcLastWriteTime.Ticks.ToString("d", NumberFormatInfo.InvariantInfo));
            //sb.Append(fileAttributesData.FileSize.ToString(CultureInfo.InvariantCulture));
        }
        private void InitUniqueID()
        {
            StringBuilder stringBuilder = null;
            object depFileInfos = _depFileInfos;
            if (depFileInfos != null)
            {
                CacheDependency.DepFileInfo depFileInfo = depFileInfos as CacheDependency.DepFileInfo;
                if (depFileInfo != null)
                {
                    stringBuilder = new StringBuilder();
                    this.AppendFileUniqueId(depFileInfo, stringBuilder);
                }
                else
                {
                    CacheDependency.DepFileInfo[] array = (CacheDependency.DepFileInfo[])depFileInfos;
                    for (int i = 0; i < array.Length; i++)
                    {
                        CacheDependency.DepFileInfo depFileInfo2 = array[i];
                        if (depFileInfo2._filename != null)
                        {
                            if (stringBuilder == null)
                            {
                                stringBuilder = new StringBuilder();
                            }
                            this.AppendFileUniqueId(depFileInfo2, stringBuilder);
                        }
                    }
                }
            }
            object entries = _entries;
            if (entries != null)
            {
                throw new NotImplementedException();
                //CacheEntry cacheEntry = entries as CacheEntry;
                //if (cacheEntry != null)
                //{
                //    if (stringBuilder == null)
                //    {
                //        stringBuilder = new StringBuilder();
                //    }
                //    stringBuilder.Append(cacheEntry.Key);
                //    stringBuilder.Append(cacheEntry.UtcCreated.Ticks.ToString(CultureInfo.InvariantCulture));
                //}
                //else
                //{
                //    CacheEntry[] array2 = (CacheEntry[])entries;
                //    for (int i = 0; i < array2.Length; i++)
                //    {
                //        CacheEntry cacheEntry2 = array2[i];
                //        if (cacheEntry2 != null)
                //        {
                //            if (stringBuilder == null)
                //            {
                //                stringBuilder = new StringBuilder();
                //            }
                //            stringBuilder.Append(cacheEntry2.Key);
                //            stringBuilder.Append(cacheEntry2.UtcCreated.Ticks.ToString(CultureInfo.InvariantCulture));
                //        }
                //    }
                //}
            }
            if (stringBuilder != null)
            {
                _uniqueID = stringBuilder.ToString();
            }
        }
        public virtual string GetUniqueID()
        {
            return _uniqueID;
        }
        protected void NotifyDependencyChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //if (_bits.ChangeValue(4, true))
            //{
            //    _utcLastModified = DateTime.UtcNow;
            //    ICacheDependencyChanged objNotify = _objNotify;
            //    if (objNotify != null && !_bits[8])
            //    {
            //        objNotify.DependencyChanged(sender, e);
            //    }
            //    this.DisposeInternal();
            //}
        }
        internal void ItemRemoved()
        {
            this.NotifyDependencyChanged(this, EventArgs.Empty);
        }
        //private void FileChange(object sender, FileChangeEvent e)
        //{
        //    this.NotifyDependencyChanged(sender, e);
        //}
        internal virtual bool IsFileDependency()
        {
            object entries = _entries;
            if (entries != null)
            {
                if (entries is CacheEntry)
                {
                    return false;
                }
                CacheEntry[] array = (CacheEntry[])entries;
                if (array != null && array.Length != 0)
                {
                    return false;
                }
            }
            object depFileInfos = _depFileInfos;
            if (depFileInfos != null)
            {
                if (depFileInfos is CacheDependency.DepFileInfo)
                {
                    return true;
                }
                CacheDependency.DepFileInfo[] array2 = (CacheDependency.DepFileInfo[])depFileInfos;
                if (array2 != null && array2.Length != 0)
                {
                    return true;
                }
            }
            return false;
        }
        internal virtual string[] GetFileDependencies()
        {
            object depFileInfos = _depFileInfos;
            if (depFileInfos == null)
            {
                return null;
            }
            CacheDependency.DepFileInfo depFileInfo = depFileInfos as CacheDependency.DepFileInfo;
            if (depFileInfo != null)
            {
                return new string[]
                {
                    depFileInfo._filename
                };
            }
            DepFileInfo[] array = (CacheDependency.DepFileInfo[])depFileInfos;
            string[] array2 = new string[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array2[i] = array[i]._filename;
            }
            return array2;
        }
    }
}
