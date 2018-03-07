
namespace BFramework.ShootingGame
{
    public class Actuator
    {
        public class PostureManager
        {
            public struct ChangeRequest
            {
                public string command;
                public string start;
                public string end;
            }
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
                StateMachine.State transition = new StateMachine.State(_tags[0], _actionTransition);
                StateMachine.State upright = new StateMachine.State(_tags[1], _actionChange);
                StateMachine.State squat = new StateMachine.State(_tags[2], _actionChange);
                StateMachine.State crawl = new StateMachine.State(_tags[3], _actionChange);
                stateMachine = new StateMachine(transition, upright, squat, crawl)
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

            public StateMachine stateMachine;
            private string[] _tags;
            private BDelegate<object, string> _actionTransition;
            private BDelegate<object, string> _actionChange;
            private ChangeRequest _request;
            private ChangeRequest _requestTemporary;
            public bool TransitionDone = true;

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
                if(_requestTemporary.end == stateMachine.Current)
                {
                    return stateMachine.Current;
                }
                else
                {
                    stateMachine.Params = _requestTemporary;
                    TransitionDone = false;
                    TransitAnimationPlay(_requestTemporary);
                    return _tags[0];
                }
            }

            private void TransitAnimationPlay(string start, string end)
            {
                //此处添加播放动画的动作
                System.Console.WriteLine("From " + start + " change to " + end);
                TransitionDone = true;
            }
            private void TransitAnimationPlay(ChangeRequest request)
            {
                //此处添加播放动画的动作
                System.Console.WriteLine("From " + request.start + " change to " + request.end);
                TransitionDone = true;
            }

            public void Run(int postureIndex)
            {
                _request.start = stateMachine.Current;
                _request.end = _tags[postureIndex];
                stateMachine.Params = _request;
                stateMachine.Run();
            }
        }

        public Actuator()
        {
            PostureMgr = new PostureManager();
        }

        private PostureManager _postureMgr;

        public PostureManager PostureMgr
        {
            get
            {
                return _postureMgr;
            }
            set
            {
                _postureMgr = value;
            }
        }

        public void Work(ref Creature.Command command)
        {
            if (command.ChangePostureTo > 0)
            {
                PostureMgr.Run(command.ChangePostureTo);
                PostureMgr.Run(command.ChangePostureTo);
            }
        }

        private bool Animation()
        {
            return true;
        }
    }
}
