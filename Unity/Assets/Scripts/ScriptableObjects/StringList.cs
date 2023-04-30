using UnityEngine;

namespace LD53.ScriptableObjects
{
    [CreateAssetMenu(menuName = "LD53/String List")]
    public class StringList : ScriptableObject
    {
        [SerializeField] private string[] _strings;

        public string[] GetAll()
        {
            return _strings;
        }

        public string GetRandom()
        {
            var index = Random.Range(0, _strings.Length);
            return _strings[index];
        }
    }
}