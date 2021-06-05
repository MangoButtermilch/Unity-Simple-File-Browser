using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Buttermilch.FileBrowser {

    public class FileBrowser : MonoBehaviour {

        private string _currentPath = "";
        [SerializeField] private string[] AllowedExtensions = { ".mp3", ".wav", ".midi" };

        private void Awake () {
            InitCurrentDirectory ();
        }

        private void InitCurrentDirectory () {
            DirectoryInfo currentDirInfo = GetCurrentDirInfo (Directory.GetCurrentDirectory ());
            UpdateCurrentPath (currentDirInfo);
        }

        public void MoveDirUp () {
            DirectoryInfo currentDirInfo = GetCurrentDirInfo (_currentPath);
            DirectoryInfo parent = currentDirInfo.Parent;
            if (parent == null) return;
            UpdateCurrentPath (parent);
        }

        public void MoveDir (string path) {
            if (string.IsNullOrEmpty (path)) return;
            DirectoryInfo directoryInfo = new DirectoryInfo (path);
            if (directoryInfo == null) return;
            UpdateCurrentPath (directoryInfo);

        }

        private void UpdateCurrentPath (DirectoryInfo directoryInfo) {
            _currentPath = directoryInfo.FullName;
        }

        private FileInfo[] GetAllFiles (DirectoryInfo directoryInfo) {
            return directoryInfo.GetFiles ();
        }

        private FileInfo[] GetAllFiles (string path) {
            if (string.IsNullOrEmpty (path)) return null;
            DirectoryInfo directoryInfo = new DirectoryInfo (path);
            return directoryInfo.GetFiles ();
        }

        private DirectoryInfo[] GetAllDirectories (DirectoryInfo directoryInfo) {
            return directoryInfo.GetDirectories ();
        }

        private DirectoryInfo[] GetAllDirectories (string path) {
            if (string.IsNullOrEmpty (path)) return null;
            DirectoryInfo directoryInfo = new DirectoryInfo (path);
            return directoryInfo.GetDirectories ();
        }

        private DirectoryInfo GetCurrentDirInfo (string path) {
            if (string.IsNullOrEmpty (path)) return null;
            return new DirectoryInfo (path);
        }

        public string[] GetAllFileNames (string path = null) {
            try {
                List<string> fileList = new List<string> ();
                FileInfo[] files = path == null ? GetAllFiles (_currentPath) : GetAllFiles (path);

                foreach (FileInfo file in files) {
                    if (AllowedExtensions.Contains (file.Extension))
                        fileList.Add (file.FullName);
                }
                return fileList.ToArray ();
            } catch (AccessViolationException e) {
                throw new AccessViolationException (e.Message);
            } catch (UnauthorizedAccessException e) {
                throw new UnauthorizedAccessException (e.Message);
            } catch (Exception e) {
                throw new Exception (e.Message);
            }
        }

        public string[] GetAllDirectoryNames (string path = null) {
            try {
                List<string> dirs = new List<string> ();
                DirectoryInfo[] directories = path == null ? GetAllDirectories (_currentPath) : GetAllDirectories (path);

                foreach (DirectoryInfo dir in directories)
                    dirs.Add (dir.FullName);

                return dirs.ToArray ();
            } catch (AccessViolationException e) {
                throw new AccessViolationException (e.Message);
            } catch (UnauthorizedAccessException e) {
                throw new UnauthorizedAccessException (e.Message);
            } catch (Exception e) {
                throw new Exception (e.Message);
            }
        }

    }
}