namespace TF.Base
{
    /// <summary>
    /// Per-level instance of all entities in the scene.
    /// </summary>
    public class EntityList
    {
        private static EntityList _instance;

        public static EntityList CreateInstance()
        {
            if (_instance is null)
                return new EntityList();
            return _instance;
        }

        private EntityList()
        {
            _instance = this;
            Precache();
        }

        public void Precache()
        {
            
        }
        
        
    }
}