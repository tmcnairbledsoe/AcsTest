using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml.Schema;

namespace AcsTest
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Score("xxxxxxxxxxxx").ToString());
        }

        public static int Score(string scoreCard)
        {
            char[] scoreCardArray = scoreCard.ToCharArray();
            List<Frame> frames = new List<Frame>();
            Frame currentFrame = new Frame(0);
            int frameIndex = 0;

            for (int i = 0; i<scoreCardArray.Length; i++)
            {
                if (frames.Count < 10)
                {
                    if (frameIndex == 0)
                    {
                        currentFrame = new Frame(i);
                    }
                    if (frameIndex < 2)
                    {
                        char currentChar = scoreCardArray[i];
                        currentFrame.FrameString = currentFrame.FrameString + currentChar;
                        frameIndex++;
                        if (currentChar.Equals('x') || frameIndex >= 2)
                        {
                            frames.Add(currentFrame);
                            frameIndex = 0;
                        }
                    }
                }
                else
                {
                    if (frameIndex == 0)
                    {
                        currentFrame = new Frame(i);
                    }
                    char currentChar = scoreCardArray[i];
                    currentFrame.FrameString = currentFrame.FrameString + currentChar;
                    frameIndex++;
                }
            }

            frames.Add(currentFrame);

            var scoreCardObject = new ScoreCard(frames, scoreCard);
            scoreCardObject.Compute();

            return scoreCardObject.Score();
        }
        

    }

    public class ScoreCard
    {
        private List<Frame> _frameList = new List<Frame>();
        private string _scoreString = "";

        public ScoreCard(List<Frame> frameList, string scoreString)
        {
            _frameList = frameList;
            _scoreString = scoreString;
        }

        public int Score()
        {
            int scoreTotal = 0;
            for(int i = 0;i< _frameList.Count; i++)
            {
                if (i != 10)
                {
                    if (!_frameList[i].HasSpare && !_frameList[i].HasStrike)
                    {
                        scoreTotal += _frameList[i].Score;
                    }
                    else if (_frameList[i].HasSpare)
                    {
                        scoreTotal += _frameList[i].Score + getSpareBonus(_frameList[i].Index);
                    }
                    else if (_frameList[i].HasStrike)
                    {
                        scoreTotal += _frameList[i].Score + getStrikeBonus(_frameList[i].Index);
                    }
                }
                else
                {
                    scoreTotal += ScoreLastFrame(_frameList[i].Index);
                }
            }
            return scoreTotal;
        }

        private int getSpareBonus(int frameIndex)
        {
            int bonus = 0;
            char[] subString = _scoreString.Substring(frameIndex+1).ToCharArray();
            if (subString.Length > 1)
            {
                if (subString[0] == 'x')
                {
                    return 10;
                }
                else
                {
                    return Convert.ToInt32(subString[0].ToString());
                }
            }
            return bonus;
        }

        private int getStrikeBonus(int frameIndex)
        {
            int bonus = 0;
            char[] subString = _scoreString.Substring(frameIndex + 1).ToCharArray();
            if (subString.Length > 2)
            {
                for (int i = 0; i<2;i++)
                {
                    if (subString[i] == 'x')
                    {
                        bonus+= 10;
                    }else if (subString[i] == '/')
                    {
                        return 10;
                    }else
                    {
                        bonus += Convert.ToInt32(subString[0].ToString());
                    }
                }
            }
            return bonus;

        }

        public int ScoreLastFrame(int frameIndex)
        {
            int currentIndex = frameIndex+1;
            int testIndex = 0;
            string subString = _scoreString.Substring(currentIndex);
            int score = 0;

            foreach (char item in subString)
            {
                if (item.Equals('x'))
                {
                    int count = 0;
                    int bonus = 0;
                    if (subString.Length > 0)
                    {
                        foreach (char item2 in subString.Substring(testIndex))
                        {
                            if (count < 2)
                            {
                                if (item2.Equals('x'))
                                {
                                    bonus += 10;
                                }
                                else if (item2.Equals('/'))
                                {
                                    bonus = 10;
                                }
                                else
                                {
                                    bonus += Convert.ToInt32(item2.ToString());
                                }
                            }
                            count++;
                        }
                    }
                    score += bonus;
                }
                else if (item.Equals('/'))
                {
                    int count = 0;
                    foreach (char item2 in subString.Substring(testIndex))
                    {
                        if (count < 1)
                        {
                            if (item2.Equals('x'))
                            {
                                score += 10;
                            }else
                            {
                                score += Convert.ToInt32(item2.ToString());
                            }
                        }
                        count++;
                    }
                }
                else
                {
                    score += Convert.ToInt32(item.ToString());
                }
                testIndex++;
            }

            return score;
        }

        public void Compute()
        {
            foreach (Frame frame in _frameList)
            {
                frame.Compute();
            }
        }
    }

    public class Frame
    {
        private string frameString = "";
        private int _score = 0;
        private int _index = 0;

        public Frame(int index)
        {
            _index = index;
        }

        public int Index
        {
            get
            {
                return _index;
            }
        }

        public bool HasStrike
        {
            get;
            set;
        }

        public bool HasSpare
        {
            get;
            set;
        }

        public string FrameString
        {
            get
            {
                return frameString;
            }
            set
            {
                frameString = value;
            }
        }

        public int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
            }
        }
        
        public void Compute()
        {
            foreach (char roll in frameString)
            {
                if (roll.Equals('x') || roll.Equals('/'))
                {
                    _score = 10;
                    if (roll.Equals('x'))
                    {
                        HasStrike = true;
                    }
                    else
                    {
                        HasSpare = true;
                    }
                }
                else
                {
                    _score = _score + Convert.ToInt32(roll.ToString());
                }
            }
        }
    }
}
