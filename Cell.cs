using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AlternativeLanguageProject
{
    class Cell
    {
        public string Oem { get; protected set; }
        public string Model { get; protected set; }
        public int LaunchAnnounced { get; protected set; }
        public string LaunchStatus { get; protected set; }
        public string BodyDimensions { get; protected set; }
        public float BodyWeight { get; protected set; }
        public string BodySim { get; protected set; }
        public string DisplayType { get; protected set; }
        public float DisplaySize { get; protected set; }
        public string DisplayResolution { get; protected set; }
        public string FeaturesSensors { get; protected set; }
        public string PlatformOS { get; protected set; }

        public Cell(String line)
        {
            ParseLine(line);
        }

        
        public void ParseLine(String line)
        {
            CharEnumerator lineReader = line.GetEnumerator();
            int counter = 0;
            String cur = "";
            
            List<String> variableList = new List<String>();
            while (lineReader.MoveNext())
            {
                char currentChar = lineReader.Current;
                char expect = ',';
                if (currentChar.Equals('\"')){
                    expect = '\"';
                    if (lineReader.MoveNext())
                    {
                        currentChar = lineReader.Current;
                    }  
                }
                while (!currentChar.Equals(expect))
                {
                    if (currentChar.Equals('\n'))
                    {
                        break;
                    }
                    cur += currentChar;
                    if (!lineReader.MoveNext())
                    {
                        break;
                    }
                    currentChar = lineReader.Current;
                }
                switch (counter)
                {

                    default:
                        String varToChange;

                        if (cur.Equals("") || cur.Equals("-"))
                        {
                            varToChange = "NoVal";
                        }
                        else
                        {
                            varToChange = cur;
                        }
                        variableList.Add(varToChange);
                        break;
                    case 2:
                        LaunchAnnounced = GetYearFromString(cur);
                        break;
                    case 3:
                        if (cur.Equals("Discontinued") || cur.Equals("Cancelled"))
                        {
                            LaunchStatus = cur;
                        }
                        else
                        {
                            int ans = GetYearFromString(cur);
                            if (ans == -1)
                            {
                                LaunchStatus = "NoVal";
                            }
                            else
                            {
                                LaunchStatus = ans.ToString();
                            }
                        }
                        break;
                    case 5:
                        BodyWeight = GetGramsFromString(cur);
                        break;
                    case 6:
                        Regex regex = new Regex(@"^[a-zA-Z]+$");
                        if (cur.Equals("No") || cur.Equals("Yes"))
                        {
                            BodySim = "noVal";
                        }
                        else if (regex.Match(cur).Success)
                        {
                            BodySim = cur;
                        }
                        else
                        {
                            BodySim = "NoVal";
                        }
                        break;
                    case 8:
                        regex = new Regex(@"^\d+(\.\d+)? inches$");
                        Regex regex2 = new Regex(@"\d+(\.\d+)?");
                        if (regex.Match(cur).Success)
                        {
               
                            String toAssign = regex.Match(cur).Value;
                            toAssign = regex2.Match(toAssign).Value;
                            DisplaySize = float.Parse(toAssign);
                        }
                        else
                        {
                            DisplaySize = -1;
                        }
                        break;
                    case 10:
                        regex = new Regex(@"^(?=.*[a-zA-Z])(?=.*\d?)[a-zA-Z\d]+$");
                        if (regex.Match(cur).Success)
                        {
                            FeaturesSensors = cur;
                        }
                        else
                        {
                            FeaturesSensors = "NoVal";
                        }
                        break;
                    case 11:
                        regex = new Regex(@"^[a-zA-Z\d. ]+$,?");
                        if (regex.Match(cur).Success)
                        {
                            PlatformOS = regex.Match(cur).ToString();
                        }
                        else
                        {
                            PlatformOS = "NoVal";
                        }
                        break;
                        
                }
                if (expect.Equals('\"'))
                {
                    lineReader.MoveNext();
                }
                cur = "";
                counter++;
            }
            Oem = variableList[0];
            Model = variableList[1];
            BodyDimensions = variableList[2];
            DisplayType = variableList[3];
            DisplayResolution = variableList[4]; 

        }


        public override string ToString()
        {

            string fullName = Oem + " " + Model;
            string toReturn = fullName;
            if(LaunchAnnounced != -1 && LaunchStatus != "NoVal")
            {
                toReturn += "\n" + "Year Announced (Launch Status): " + LaunchAnnounced + "(" + LaunchStatus + ")" + "\n";
            }
            if(BodyWeight != -1 && BodyDimensions != "NoVal")
            {
                toReturn += "Weight, Dimensions: " + BodyWeight + ", " + BodyDimensions + "\n";
            }
            if(DisplayType != "NoVal" && DisplayResolution != "NoVal" && DisplaySize != -1)
            {
                toReturn += "Display Type, Size, Resolution: " + DisplayType + "," + DisplaySize + "," + DisplayResolution + "\n";
            }
            if(BodySim != "NoVal" && PlatformOS != "NoVal")
            {
                toReturn += "Sim Type - Operating System: " + BodySim + " - " + PlatformOS;
            }

            return toReturn + "\n";
                
           

            
        }

        public int GetYearFromString(String s)
        {
            Regex regex = new Regex(@"\b\d{4}\b");
            int ans;
            string q = "z";
            if (regex.Match(s).Success)
            {
                q = regex.Match(s).ToString();
            }
            if (int.TryParse(q, out ans))
            {
                return ans;
            }
            return -1;
        }

        public float GetGramsFromString(String s)
        {
            Regex validWeight = new Regex(@"\d+\s*[g]{1}");
            Regex weightNoG = new Regex(@"\d+");
            if (validWeight.IsMatch(s))
            {
                return float.Parse(weightNoG.Match(validWeight.Match(s).ToString()).ToString());
            }
            else
            {
                return -1;
            }
        }
    }
}
