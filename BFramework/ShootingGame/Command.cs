using System.Collections.Generic;

namespace BFramework.ShootingGame
{

    /// <summary>
    /// 定义按键类需要具有的属性及方法
    /// </summary>
    public interface IButton
    {

        /// <summary>
        /// 按住状态
        /// </summary>
        bool Hold { get; }

        /// <summary>
        /// 按下状态
        /// </summary>
        bool Down { get; }

        /// <summary>
        /// 按下后弹起的状态
        /// </summary>
        bool Up { get; }

        /// <summary>
        /// 按键的向量输入
        /// </summary>
        Vector Vector { get; }

        /// <summary>
        /// 按键的向量输入长度
        /// </summary>
        float VectorLength { get; }

        /// <summary>
        /// 取得输入
        /// </summary>
        void Capture();
    }

    /// <summary>
    /// 定义平面二维向量输入按键需要具备的属性
    /// </summary>
    public interface IVector2Panel : IButton
    {

        /// <summary>
        /// 水平方向摇杆的名称
        /// </summary>
        string HorizontalName { get; }

        /// <summary>
        /// 竖直方向摇杆的名称
        /// </summary>
        string VerticalName { get; }
    }

    /// <summary>
    /// 定义虚拟按键需要具备的属性
    /// </summary>
    public interface IVirtualButton : IButton
    {

        /// <summary>
        /// 是否与实体按键冲突
        /// </summary>
        bool IncompatibleWithRealButton { get; set; }

        /// <summary>
        /// 是否与实体按键绑定
        /// </summary>
        bool IsConnectedWithButton { get; set; }

        /// <summary>
        /// 与之绑定的实体按键
        /// </summary>
        ButtonBase ConnectedWith { get; set; }
    }

    /// <summary>
    /// 按键基类
    /// </summary>
    public abstract class ButtonBase : IButton
    {

        /// <summary>
        /// 名称
        /// </summary>
        public string name;

        /// <summary>
        /// 按键是否被按住
        /// </summary>
        public bool Hold { get; set; }

        /// <summary>
        /// 按键是否被按下
        /// </summary>
        public bool Down { get; set; }

        /// <summary>
        /// 按键是否弹起
        /// </summary>
        public bool Up { get; set; }

        /// <summary>
        /// 按键上的向量输入
        /// </summary>
        public Vector Vector { get; set; }

        /// <summary>
        /// 按键上向量输入的长度
        /// </summary>
        public float VectorLength { get; set; }

        /// <summary>
        /// 按键所绑定的虚拟按键是否与实体键冲突
        /// </summary>
        protected bool _incompatibleWithRealButton = false;

        /// <summary>
        /// 与按键绑定的虚拟按键
        /// </summary>
        protected IVirtualButton _virtualButton;

        /// <summary>
        /// 是否有虚拟按键与此绑定
        /// </summary>
        public virtual bool HasVirtualButton { get { return _virtualButton != null; } }

        /// <summary>
        /// 取得输入
        /// </summary>
        public abstract void Capture();

        /// <summary>
        /// 与虚拟按键绑定
        /// </summary>
        /// <param name="button"></param>
        public virtual void ConnectWith(IVirtualButton button)
        {
            if (button == null) { return; }
            _virtualButton = button;
            _incompatibleWithRealButton = _virtualButton.IncompatibleWithRealButton;
            button.IsConnectedWithButton = true;
            button.ConnectedWith = this;
        }

        /// <summary>
        /// 解除与虚拟按键的绑定
        /// </summary>
        public virtual void Disconnect()
        {
            _virtualButton = null;
            _incompatibleWithRealButton = false;
        }
    }

    /// <summary>
    /// 输入指令集的基类
    /// </summary>
    public class CommandBase
    {

        /// <summary>
        /// 指令集的所有按键
        /// </summary>
        protected List<ButtonBase> _buttons;

        /// <summary>
        /// 构建一个指令集
        /// </summary>
        /// <param name="buttons"></param>
        public CommandBase(List<ButtonBase> buttons)
        {
            _buttons = buttons;
        }

        /// <summary>
        /// 构建一个空指令集
        /// </summary>
        public CommandBase() : this(new List<ButtonBase>()) { }

        /// <summary>
        /// 添加按键
        /// </summary>
        /// <param name="button"></param>
        public void AddButton(ButtonBase button)
        {
            _buttons.Add(button);
        }

        /// <summary>
        /// 获取输入
        /// </summary>
        public void Capture()
        {
            foreach (var item in _buttons)
            {
                item.Capture();
            }
        }
    }
}
