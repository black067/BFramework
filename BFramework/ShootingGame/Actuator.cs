using BFramework.StateMachines;
namespace BFramework.ShootingGame
{
    public class Actuator
    {
        private class ChangeRequest
        {
            public string command;
            public string start;
            public string end;
        }
        public class PostureManager
        {
            public PostureManager()
            {
                _tags = new string[]
                {
                    //0
                    Creature.Attribute.POSTURE.TRANSITION.ToString(),
                    //1
                    Creature.Attribute.POSTURE.UPRIGHT.ToString(),
                    //2
                    Creature.Attribute.POSTURE.SQUAT.ToString(),
                    //3
                    Creature.Attribute.POSTURE.CRAWL.ToString(),
                };
                _actionTransition = new BDelegate<object, string>(Transition);
                _actionChange = new BDelegate<object, string>(ChangeTo);
                State transition = new State(_tags[0], _actionTransition);
                State upright = new State(_tags[1], _actionChange);
                State squat = new State(_tags[2], _actionChange);
                State crawl = new State(_tags[3], _actionChange);
                StateMachine = new StateMachine(transition, upright, squat, crawl)
                {
                    Current = _tags[1],
                    Params = _tags[1]
                };
                _request = new ChangeRequest()
                {
                    start = _tags[1],
                    end = _tags[1],
                    command = _tags[1]
                };
            }

            public StateMachine StateMachine { get; set; }
            private string[] _tags { get; set; }
            private BDelegate<object, string> _actionTransition { get; set; }
            private BDelegate<object, string> _actionChange { get; set; }
            private ChangeRequest _request;
            private ChangeRequest _requestTemporary;
            public bool TransitionDone { get; set; } = true;

            private string Transition()
            {
                if (TransitionDone)//此处判断过渡动画是否播放完毕
                {
                    TransitionDone = false;
                    return _request.end;
                }
                else
                {
                    return _tags[0];
                }
            }

            private string ChangeTo(ref object input)
            {
                _requestTemporary = (ChangeRequest)input;
                
                if(_requestTemporary.end == StateMachine.Current)
                {
                    return StateMachine.Current;
                }
                else
                {
                    StateMachine.Params = _requestTemporary;
                    TransitionDone = false;
                    TransitAnimationPlay(_requestTemporary);
                    return _tags[0];
                }
            }
            
            private void TransitAnimationPlay(ChangeRequest request)
            {
                //此处添加播放动画的动作
                System.Console.WriteLine("From " + request.start + " change to " + request.end);
                TransitionDone = true;
            }

            public void Run(int postureIndex)
            {
                if (postureIndex > 0 && postureIndex < 4)
                {
                    _request.start = StateMachine.Current;
                    if (_tags[postureIndex] == StateMachine.Current)
                    {
                        _request.end = _tags[1];
                    }
                    else
                    {
                        _request.end = _tags[postureIndex];
                    }
                }
                StateMachine.Params = _request;
                StateMachine.Run();
            }

            public void ChangeAction(string tag, BDelegate<object, string> action)
            {
                StateMachine.States[tag].Action = action;
            }
        }

        public Actuator()
        {
            PostureMgr = new PostureManager();
        }
        
        public PostureManager PostureMgr { get; set; }

        public void Work(ref Creature.Command command)
        {
            PostureMgr.Run(command.ChangePostureTo);
        }

        private bool Animation()
        {
            return true;
        }
    }
}
