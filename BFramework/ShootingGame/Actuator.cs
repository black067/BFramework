using BFramework;

namespace BFramework.ShootingGame
{
    class Actuator
    {
        class PostureManager
        {
            public PostureManager()
            {
                _tags = new string[]
                {
                    //0
                    Creature.Attributes.POSTURE.TRANSITION.ToString(),
                    //1
                    Creature.Attributes.POSTURE.UPRIGHT.ToString(),
                    //2
                    Creature.Attributes.POSTURE.SQUAT.ToString(),
                    //3
                    Creature.Attributes.POSTURE.CRAWL.ToString(),
                };
                _actions = new BDelegate<object, string>[]
                {
                    new BDelegate<object, string>(Transition),
                    new BDelegate<object, string>(Upright),
                    new BDelegate<object, string>(Squat),
                    new BDelegate<object, string>(Crawl),
                };
                StateMachine.State transition = new StateMachine.State(_tags[0], _actions[0]);
                StateMachine.State upright = new StateMachine.State(_tags[1], _actions[1]);
                StateMachine.State squat = new StateMachine.State(_tags[2], _actions[2]);
                StateMachine.State crawl = new StateMachine.State(_tags[3], _actions[3]);
                _stateMachine = new StateMachine(transition, upright, squat, crawl);
            }

            private StateMachine _stateMachine;
            private string[] _tags;
            private BDelegate<object, string>[] _actions;
            private string _command;
            public bool TransitionDone;

            private string Transition(ref object command)
            {
                if (TransitionDone)//此处判断过渡动画是否播放完毕
                {
                    return (string)_stateMachine.Params;
                }
                else
                {
                    return _tags[0];
                }
            }
            private string Upright(ref object command)
            {
                _stateMachine.Params = command;
                _command = (string)command;
                if (_command == _tags[1])
                {
                    return _tags[1];
                }
                else
                {
                    TransitionDone = false;
                    TransitAnimationPlay(_tags[1], _command);
                    return _tags[0];
                }
            }
            private string Squat(ref object command)
            {
                _stateMachine.Params = command;
                _command = (string)command;
                if (_command == _tags[2])
                {
                    return _tags[2];
                }
                else
                {
                    TransitionDone = false;
                    TransitAnimationPlay(_tags[2], _command);
                    return _tags[0];
                }
            }
            private string Crawl(ref object command)
            {
                _stateMachine.Params = command;
                _command = (string)command;
                if (_command == _tags[3])
                {
                    return _tags[3];
                }
                else
                {
                    TransitionDone = false;
                    TransitAnimationPlay(_tags[3], _command);
                    return _tags[0];
                }
            }

            private void TransitAnimationPlay(string start, string end)
            {
                //此处添加播放动画的动作
                TransitionDone = true;
            }

            public void Run(int postureIndex)
            {
                _stateMachine.Run(_tags[postureIndex]);
            }
        }

        public Actuator()
        {
            _postureManager = new PostureManager();
        }

        private PostureManager _postureManager;
        
        public void Work(ref Creature.Command command)
        {
            if (command.ChangePostureTo > 0)
            {
                _postureManager.Run(command.ChangePostureTo);
            }
        }
    }
}
