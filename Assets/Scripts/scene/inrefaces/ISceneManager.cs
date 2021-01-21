using Assets.Scripts.item;
using UnityEngine;

namespace Assets.Scripts.scene.inrefaces
{
    public interface ISceneManager
    {

        bool CanDragItemToPosition(IRectItem draggingItem, Vector3 toPosition);

    }
}
