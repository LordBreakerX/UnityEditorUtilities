using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace LordBreakerX.EditorUtilities
{
    public class ObjectPanel<TObject> : SectionPanel where TObject : Object
    {
        public ObjectPanel() : base()
        {
            
        }

        public ObjectPanel(string headerTitle) : base(headerTitle)
        {

        }

        public void SetObject(TObject obj)
        {
            Clear();

            if (obj != null)
            {
                var serializedObject = new SerializedObject(obj);

                InspectorElement inspectorElement = new InspectorElement(serializedObject);

                Add(inspectorElement);

                SetEnabled(true);
            }
            else
            {
                SetEnabled(false);
            }
        }
    }

    [UxmlElement]
    public partial class ObjectPanel : ObjectPanel<Object>
    {
        public ObjectPanel() : base()
        {

        }

        public ObjectPanel(string headerTitle) : base(headerTitle)
        {

        }
    }
}
