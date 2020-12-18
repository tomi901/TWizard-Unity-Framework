using UnityEngine;
using UnityEngine.Events;


namespace TWizard.Core
{
    public class VersionText : MonoBehaviour
    {
        [System.Serializable]
        protected class SetTextEvent : UnityEvent<string> { }

        [SerializeField]
        private string format = "v. {0}";
        public string Text => Get(format);

        [SerializeField]
        private SetTextEvent setText = default;
        public event UnityAction<string> SetText
        {
            add => setText.AddListener(value);
            remove => setText.RemoveListener(value);
        }

        private void Start()
        {
            setText.Invoke(Text);
        }


        public string Get(string format) => string.Format(format, Application.version);
    }
}
