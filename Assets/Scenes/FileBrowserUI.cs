using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Buttermilch.FileBrowser {
    public class FileBrowserUI : MonoBehaviour {

        [SerializeField] private GameObject _directoryPrefab;
        [SerializeField] private FileBrowser _fileBrowser;
        [SerializeField] private RectTransform _contentView;
        [SerializeField] private RectTransform _directoryPlaceHolder;
        [SerializeField] private float _placeholderSpacing = 40;

        [SerializeField] private Color _directoryColor;
        [SerializeField] private Color _fileColor;

        private void Start () {
            BuildDirectoryView ();
        }

        private void BuildDirectoryView (string path = null) {
            string[] directories = _fileBrowser.GetAllDirectoryNames (path);
            string[] files = _fileBrowser.GetAllFileNames (path);
            int i = 0;

            foreach (string dir in directories) {
                GameObject go = Instantiate (_directoryPrefab, _directoryPlaceHolder.anchoredPosition + (Vector2.down * _placeholderSpacing * i), Quaternion.identity);
                go.transform.SetParent (_contentView, false);
                go.GetComponentInChildren<TextMeshProUGUI> ().text = dir;
                go.GetComponent<Image>().color = _directoryColor;
                go.GetComponentInChildren<Button> ().onClick.AddListener (() => { MoveDir (dir); });
                i++;
            }
            foreach (string file in files) {
                GameObject go = Instantiate (_directoryPrefab, _directoryPlaceHolder.anchoredPosition + (Vector2.down * _placeholderSpacing * i), Quaternion.identity);
                go.transform.SetParent (_contentView, false);
                go.GetComponentInChildren<TextMeshProUGUI> ().text = file;
                go.GetComponent<Image>().color = _fileColor;
                go.GetComponentInChildren<Button> ().onClick.AddListener (() => { Debug.Log ("selected file: " + file); });
                i++;
            }
        }

        private void DestroyDirectoryView () {
            RectTransform[] childs = _contentView.GetComponentsInChildren<RectTransform> ();

            foreach (RectTransform go in childs) {
                if (go != null && !go.name.Equals ("DirectoryPlaceHolder") && !go.name.Equals ("Content"))
                    Destroy (go.gameObject);
            }
        }

        public void MoveDirUp () {
            _fileBrowser.MoveDirUp ();
            DestroyDirectoryView ();
            BuildDirectoryView ();
        }

        public void MoveDir (string path) {
            _fileBrowser.MoveDir (path);
            DestroyDirectoryView ();
            BuildDirectoryView (path);
        }

    }
}