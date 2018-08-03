using System.Collections.Generic;

namespace BFramework.ShootingGame
{
    public interface IButton
    {
        bool Hold { get; }
        bool Down { get; }
        bool Up { get; }
        Vector Vector { get; }
        float VectorLength { get; }
        void Capture();
    }

    public interface IVector2Panel : IButton
    {
        string HorizontalName { get; }
        string VerticalName { get; }
    }

    public interface IVirtualButton : IButton
    {
        bool IncompatibleWithRealButton { get; set; }
        bool IsConnectedWithButton { get; set; }
        ButtonBase ConnectedWith { get; set; }
    }

    public abstract class ButtonBase : IButton
    {
        public string name;
        protected bool _hold;
        public bool Hold { get { return _hold; } set { _hold = value; } }
        protected bool _down;
        public bool Down { get { return _down; } set { _down = value; } }
        protected bool _up;
        public bool Up { get { return _up; } set { _up = value; } }
        protected Vector _vector;
        public Vector Vector { get { return _vector; } set { _vector = value; } }
        protected float _vectorLength;
        public float VectorLength { get { return _vectorLength; } set { _vectorLength = value; } }

        protected bool _incompatibleWithRealButton = false;
        protected IVirtualButton _virtualButton;
        public bool HasVirtualButton { get { return _virtualButton != null; } }

        public abstract void Capture();

        public virtual void ConnectWith(IVirtualButton button)
        {
            if (button == null) { return; }
            _virtualButton = button;
            _incompatibleWithRealButton = _virtualButton.IncompatibleWithRealButton;
            button.IsConnectedWithButton = true;
            button.ConnectedWith = this;
        }
        public virtual void Disconnect()
        {
            _virtualButton = null;
            _incompatibleWithRealButton = false;
        }
    }

    public class CommandBase
    {
        protected List<ButtonBase> _buttons;

        public CommandBase(List<ButtonBase> buttons)
        {
            _buttons = buttons;
        }

        public CommandBase() : this(new List<ButtonBase>()) { }

        public void AddButton(ButtonBase button)
        {
            _buttons.Add(button);
        }

        public void Capture()
        {
            foreach (var item in _buttons)
            {
                item.Capture();
            }
        }
    }
}
