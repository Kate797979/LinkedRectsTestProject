using UnityEngine;

namespace Assets.Scripts.item.interfaces
{
    public interface IRectItemsLink
    {

        GameObject gameObject { get; }

        RectTransform rectTransform { get; }

        /// <summary>
        /// Объект, участвующий в связи
        /// </summary>
        IRectItem item1 { get; }

        /// <summary>
        /// Объект, участвующий в связи
        /// </summary>
        IRectItem item2 { get; }

        /// <summary>
        /// Создаёт связь между объектами
        /// </summary>
        /// <param name="item1">Первый объект, участвующий в связи</param>
        /// <param name="item2">Второй объект, участвующий в связи</param>
        void CreateLink(IRectItem item1, IRectItem item2);
       
        /// <summary>
        /// Удаляет связь между объектыми
        /// </summary>
        void DestroyLink();

        /// <summary>
        /// Обновляет визуализацию из текущих позиций связанных объектов (когда перемещаем один из связанных объектов)
        /// </summary>
        void UpdateTransformFromItems();

        void UpdateTransformFromLinePoints(Vector3 lineStartPosution, Vector3 lineEndPosution);

        void SetAlpha(float alpha);

    }
}
