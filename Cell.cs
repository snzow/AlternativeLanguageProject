using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlternativeLanguageProject
{
    class Cell
    {
        String oem;
        String model;
        int launchAnnounced;
        String launchStatus;
        String bodyDimensions;
        String bodyWeight;
        String bodySim;
        String displayType;
        float displaySize;
        String displayResolution;
        String featuresSensors;
        String platformOS;

        public Cell(String line)
        {
            ParseLine(line); 
        }
        
        public void ParseLine(String line)
        {
            CharEnumerator lineReader = line.GetEnumerator();
            int counter = 0;
            String cur = "";
            char currentChar = lineReader.Current;
            List<String> variableList = new List<String>();
            variableList.Add(oem);
            variableList.Add(model);
           
            variableList.Add(bodyDimensions);

            while (!currentChar.Equals('\n'))
            {
                while (!currentChar.Equals(','))
                {
                    if (currentChar.Equals('\n'))
                    {
                        break;
                    }
                    cur += currentChar;
                    lineReader.MoveNext();
                }
                switch (counter)
                {

                    //cases with differences. 2,3
                    default:
                        String varToChange;
                        
                        if (cur.Equals(""))
                        {
                            varToChange = null;
                        }
                        else
                        {
                            varToChange = cur;
                        }
                        break;
                    case 2:
                        launchAnnounced = GetYearFromString(cur);
                        break;
                    case 3:
                        if(cur.Equals("Discontinued") || cur.Equals("Cancelled"))
                        {
                            launchStatus = cur;
                        }
                        else
                        {
                            int ans = GetYearFromString(cur);
                            if(ans == -1)
                            {
                                launchStatus = null;
                            }
                            else
                            {
                                launchStatus = ans.ToString();
                            };
                        }
                        break;
                    case 4:

                        break;

                }
                cur = "";
            }
        }

        public int GetYearFromString(String s)
        {
            CharEnumerator ch = s.GetEnumerator();
            int ans;
            String nums = "";
            char c = ch.Current;
            if(int.TryParse(ch.Current.ToString(),out ans))
            {
                nums += ch.Current;
            }
            while (ch.MoveNext())
            {
                if(int.TryParse(ch.Current.ToString(),out ans))
                {
                    nums += ch.Current;
                }
            }
            if (int.TryParse(s, out ans) && (ans > 999 && ans < 10000))
            {
                return ans;
            }
            return -1;
        }

        public float SetDisplaySize(String s)
        {

        }
    }
}
