using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace DDB.Bindings
{
    public static class DroneDB
    {
        [DllImport("ddb", EntryPoint = "DDBRegisterProcess")]
        public static extern void RegisterProcess(bool verbose = false);

        [DllImport("ddb", EntryPoint = "DDBGetVersion")]
        private static extern IntPtr _GetVersion();

        public static string GetVersion()
        {
            var ptr = _GetVersion();
            return Marshal.PtrToStringAnsi(ptr);
        }

        [DllImport("ddb", EntryPoint = "DDBGetLastError")]
        private static extern IntPtr _GetLastError();

        static string GetLastError()
        {
            var ptr = _GetLastError();
            return Marshal.PtrToStringAnsi(ptr);
        }

        [DllImport("ddb", EntryPoint = "DDBInit")]
        private static extern DDBError _Init([MarshalAs(UnmanagedType.LPStr)] string directory, out IntPtr outPath);

        public static string Init(string directory)
        {

            try
            {

                if (_Init(directory, out var outPath) == DDBError.DDBERR_NONE)
                    return Marshal.PtrToStringAnsi(outPath);

            }
            catch (Exception ex)
            {
                throw new DDBException($"Error in calling ddb lib. Last error: \"{GetLastError()}\", check inner exception for details", ex);
            }

            throw new DDBException(GetLastError());


        }

        [DllImport("ddb", EntryPoint = "DDBAdd")]
        private static extern DDBError _Add([MarshalAs(UnmanagedType.LPStr)] string ddbPath,
                                  [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] paths,
                                  int numPaths, out IntPtr output, bool recursive);

        public static List<Entry> Add(string ddbPath, string path, bool recursive = false)
        {
            return Add(ddbPath, path != null ? new[] { path } : null, recursive);
        }

        public static List<Entry> Add(string ddbPath, string[] paths, bool recursive = false)
        {

            try
            {
                if (_Add(ddbPath, paths, paths?.Length ?? 0, out var output, recursive) != DDBError.DDBERR_NONE)
                    throw new DDBException(GetLastError());

                var json = Marshal.PtrToStringAnsi(output);

                if (string.IsNullOrWhiteSpace(json))
                    throw new DDBException("Unable to add");

                return JsonConvert.DeserializeObject<List<Entry>>(json);
            }
            catch (Exception ex)
            {
                throw new DDBException($"Error in calling ddb lib. Last error: \"{GetLastError()}\", check inner exception for details", ex);
            }

        }

        [DllImport("ddb", EntryPoint = "DDBRemove")]
        private static extern DDBError _Remove([MarshalAs(UnmanagedType.LPStr)] string ddbPath,
                                  [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] paths,
                                  int numPaths);

        public static void Remove(string ddbPath, string path)
        {
            Remove(ddbPath, path != null ? new[] { path } : null);
        }
        public static void Remove(string ddbPath, string[] paths)
        {
            try
            {
                if (_Remove(ddbPath, paths, paths?.Length ?? 0) != DDBError.DDBERR_NONE)
                    throw new DDBException(GetLastError());
            }
            catch (Exception ex)
            {
                throw new DDBException($"Error in calling ddb lib. Last error: \"{GetLastError()}\", check inner exception for details", ex);
            }
        }

        [DllImport("ddb", EntryPoint = "DDBInfo")]
        private static extern DDBError _Info([MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] paths,
                                   int numPaths,
                                   out IntPtr output,
                                   [MarshalAs(UnmanagedType.LPStr)] string format, bool recursive = false,
                                   int maxRecursionDepth = 0, [MarshalAs(UnmanagedType.LPStr)] string geometry = "auto",
                                   bool withHash = false, bool stopOnError = true);

        public static List<Entry> Info(string path, bool recursive = false, int maxRecursionDepth = 0, bool withHash = false)
        {
            return Info(path != null ? new[] { path } : null, recursive, maxRecursionDepth, withHash);
        }

        public static List<Entry> Info(string[] paths, bool recursive = false, int maxRecursionDepth = 0, bool withHash = false)
        {

            try
            {
                if (_Info(paths, paths?.Length ?? 0, out var output, "json", recursive, maxRecursionDepth, "auto", withHash) !=
                    DDBError.DDBERR_NONE) throw new DDBException(GetLastError());

                var json = Marshal.PtrToStringAnsi(output);

                if (string.IsNullOrWhiteSpace(json))
                    throw new DDBException("Unable get info");

                return JsonConvert.DeserializeObject<List<Entry>>(json);

            }
            catch (Exception ex)
            {
                throw new DDBException($"Error in calling ddb lib. Last error: \"{GetLastError()}\", check inner exception for details", ex);
            }
        }

        [DllImport("ddb", EntryPoint = "DDBList")]
        private static extern DDBError _List([MarshalAs(UnmanagedType.LPStr)] string ddbPath,
                                    [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPStr)] string[] paths,
                                    int numPaths,
                                    out IntPtr output,
                                    [MarshalAs(UnmanagedType.LPStr)] string format,
                                    bool recursive,
                                    int maxRecursionDepth = 0);

        public static List<Entry> List(string ddbPath, string path, bool recursive = false, int maxRecursionDepth = 0)
        {
            return List(ddbPath, path != null ? new[] { path } : null, recursive, maxRecursionDepth);
        }

        public static List<Entry> List(string ddbPath, string[] paths, bool recursive = false, int maxRecursionDepth = 0)
        {
            try
            {

                if (_List(ddbPath, paths, paths?.Length ?? 0, out var output, "json", recursive, maxRecursionDepth) !=
                    DDBError.DDBERR_NONE) throw new DDBException(GetLastError());

                var json = Marshal.PtrToStringAnsi(output);

                if (string.IsNullOrWhiteSpace(json))
                    throw new DDBException("Unable get list");

                return JsonConvert.DeserializeObject<List<Entry>>(json);

            }
            catch (Exception ex)
            {
                throw new DDBException($"Error in calling ddb lib. Last error: \"{GetLastError()}\", check inner exception for details", ex);
            }

        }

        [DllImport("ddb", EntryPoint = "DDBAppendPassword")]
        private static extern DDBError _AppendPassword(
            [MarshalAs(UnmanagedType.LPStr)] string ddbPath, 
            [MarshalAs(UnmanagedType.LPStr)] string password);

        public static void AppendPassword(string ddbPath, string password)
        {
            try
            {

                if (_AppendPassword(ddbPath, password) !=
                    DDBError.DDBERR_NONE) throw new DDBException(GetLastError());
                
            }
            catch (Exception ex)
            {
                throw new DDBException($"Error in calling ddb lib. Last error: \"{GetLastError()}\", check inner exception for details", ex);
            }
        }

        [DllImport("ddb", EntryPoint = "DDBVerifyPassword")]
        static extern DDBError _VerifyPassword(
            [MarshalAs(UnmanagedType.LPStr)] string ddbPath,
            [MarshalAs(UnmanagedType.LPStr)] string password,
            out bool verified);

        public static bool VerifyPassword(string ddbPath, string password)
        {
            try
            {

                if (_VerifyPassword(ddbPath, password, out var res) !=
                    DDBError.DDBERR_NONE) throw new DDBException(GetLastError());

                return res;
            }
            catch (Exception ex)
            {
                throw new DDBException($"Error in calling ddb lib. Last error: \"{GetLastError()}\", check inner exception for details", ex);
            }

        }

        [DllImport("ddb", EntryPoint = "DDBClearPasswords")]
        static extern DDBError _ClearPasswords (
            [MarshalAs(UnmanagedType.LPStr)] string ddbPath);

        public static void ClearPasswords(string ddbPath)
        {
            try
            {

                if (_ClearPasswords(ddbPath) !=
                    DDBError.DDBERR_NONE) throw new DDBException(GetLastError());

            }
            catch (Exception ex)
            {
                throw new DDBException($"Error in calling ddb lib. Last error: \"{GetLastError()}\", check inner exception for details", ex);
            }

        }

        [DllImport("ddb", EntryPoint = "DDBChattr")]
        static extern DDBError _ChangeAttributes(
            [MarshalAs(UnmanagedType.LPStr)] string ddbPath, [MarshalAs(UnmanagedType.LPStr)] string attributesJson, out IntPtr jsonOutput);

        public static Dictionary<string, object> ChangeAttributes(string ddbPath, Dictionary<string, object> attributes)
        {

            if (attributes == null)
                throw new ArgumentException("Attributes is null");

            try
            {

                var attrs = JsonConvert.SerializeObject(attributes);

                if (_ChangeAttributes(ddbPath, attrs, out var output) !=
                    DDBError.DDBERR_NONE) throw new DDBException(GetLastError());

                var res = Marshal.PtrToStringAnsi(output);

                if (string.IsNullOrWhiteSpace(res))
                    throw new DDBException("Unable get attributes");

                return JsonConvert.DeserializeObject<Dictionary<string, object>>(res);

            }
            catch (Exception ex)
            {
                throw new DDBException($"Error in calling ddb lib. Last error: \"{GetLastError()}\", check inner exception for details", ex);
            }

        }

        public static Dictionary<string, object> GetAttributes(string ddbPath)
        {
            return ChangeAttributes(ddbPath, new Dictionary<string, object>());
        }

        [DllImport("ddb", EntryPoint = "DDBGenerateThumbnail")]
        static extern DDBError _GenerateThumbnail(
            [MarshalAs(UnmanagedType.LPStr)] string filePath, int size, [MarshalAs(UnmanagedType.LPStr)] string destPath);

        public static void GenerateThumbnail(string filePath, int size, string destPath)
        {

            if (filePath == null)
                throw new ArgumentException("filePath is null");

            if (destPath == null)
                throw new ArgumentException("destPath is null");

            try
            {

                if (_GenerateThumbnail(filePath, size, destPath) !=
                    DDBError.DDBERR_NONE) throw new DDBException(GetLastError());
                
            }
            catch (Exception ex)
            {
                throw new DDBException($"Error in calling ddb lib. Last error: \"{GetLastError()}\", check inner exception for details", ex);
            }

        }

        [DllImport("ddb", EntryPoint = "DDBTile")]
        static extern DDBError _GenerateTile(
            [MarshalAs(UnmanagedType.LPStr)] string geotiffPath, int tz, int tx, int ty, out IntPtr outputTilePath, int tileSize, bool tms, bool forceRecreate);

        public static string GenerateTile(string filePath, int tz, int tx, int ty, int tileSize, bool tms, bool forceRecreate = false)
        {

            if (filePath == null)
                throw new ArgumentException("filePath is null");

            try
            {

                if (_GenerateTile(filePath, tz, tx, ty, out var output, tileSize, tms, forceRecreate) !=
                    DDBError.DDBERR_NONE) throw new DDBException(GetLastError());

                var res = Marshal.PtrToStringAnsi(output);

                if (string.IsNullOrWhiteSpace(res))
                    throw new DDBException("Unable get tile path");

                return res;

            }
            catch (Exception ex)
            {
                throw new DDBException($"Error in calling ddb lib. Last error: \"{GetLastError()}\", check inner exception for details", ex);
            }

        }


    }

}
