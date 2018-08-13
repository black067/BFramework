using System.Collections;
using System.Collections.Generic;

namespace BFramework.ShootingGame
{

    /// <summary>
    /// 定义物品持有者需要带有的方法
    /// </summary>
    public interface IItemHolder
    {

        /// <summary>
        /// 取得某个道具
        /// </summary>
        /// <param name="item"></param>
        void GetItem(IItemBehaviour item);

        /// <summary>
        /// 掉落/失去某个道具
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        IItemBehaviour DropItem(IItemBehaviour item);
    }

    /// <summary>
    /// 道具组件需要带有的属性与方法
    /// </summary>
    public interface IItemComponent
    {

        /// <summary>
        /// 组件的顺序, Order 越小的组件越先执行
        /// </summary>
        int Order { get; }

        /// <summary>
        /// 组件是否已初始化
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        /// 初始化组件
        /// </summary>
        void Init();

        /// <summary>
        /// 调用组件, 使其发挥功能
        /// </summary>
        void Tick();

        /// <summary>
        /// 组件所依附的 ItemBehaviour
        /// </summary>
        IItemBehaviour BehaviourAttached { get; set; }
    }

    /// <summary>
    /// 道具类需要带有的属性与方法
    /// </summary>
    public interface IItemBehaviour
    {

        /// <summary>
        /// 道具是否可用
        /// </summary>
        bool Active { get; }

        /// <summary>
        /// 道具的信息
        /// </summary>
        ItemInfo Info { get; }

        /// <summary>
        /// 道具持有者
        /// </summary>
        IItemHolder Holder { get; }

        /// <summary>
        /// 道具的组件列表
        /// </summary>
        List<IItemComponent> Components { get; }

        /// <summary>
        /// 为道具添加组件, 相同类型的组件只能存在一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T AddComponent<T>() where T : class, IItemComponent;

        /// <summary>
        /// 为道具移除组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T RemoveComponent<T>() where T : class, IItemComponent;

        /// <summary>
        /// 尝试取得指定类型的组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        bool TryGetComponent<T>(out T result) where T : class, IItemComponent;

        /// <summary>
        /// 道具被持有者取得
        /// </summary>
        /// <param name="holder"></param>
        void GetBy(IItemHolder holder);

        /// <summary>
        /// 道具被掉落/失去
        /// </summary>
        void Drop();

        /// <summary>
        /// 使用道具
        /// </summary>
        void Tick();

        /// <summary>
        /// 开启一个协程
        /// </summary>
        /// <param name="enumerator"></param>
        void StartCoroutine(IEnumerator enumerator);
    }
}
