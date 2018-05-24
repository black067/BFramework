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

                State transition = new State(_tags[0], new BDelegate<object, string>(Transition));
                State upright = new State(_tags[1], ChangeTo);
                State squat = new State(_tags[2], ChangeTo);
                State crawl = new State(_tags[3], ChangeTo);
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

            public StateMachine StateMachine
            {
                get; set;
            }
            private string[] _tags
            {
                get; set;
            }
            public BDelegate AnimationPlayer
            {
                get; set;
            }

            private ChangeRequest _request;

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

            private string ChangeTo(object input)
            {
                ChangeRequest request = (ChangeRequest)input;

                if (request.end == StateMachine.Current)
                {
                    return StateMachine.Current;
                }
                else
                {
                    StateMachine.Params = request;
                    TransitionDone = false;
                    TransitAnimationPlay(request);
                    return _tags[0];
                }
            }

            public string Action(object reqObject)
            {
                ChangeRequest request = reqObject as ChangeRequest;
                return StateMachine.Current;
            }

            private void TransitAnimationPlay(ChangeRequest request)
            {
                AnimationPlayer.Execute();
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

        public PostureManager PostureMgr
        {
            get; set;
        }

        public void Work(ref Creature.Command command)
        {
            PostureMgr.Run(command.ChangePostureTo);
        }
    }
}
