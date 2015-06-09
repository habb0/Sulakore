/* Copyright

    GitHub(Source): https://GitHub.com/ArachisH/Sulakore

    .NET library for creating Habbo Hotel related desktop applications.
    Copyright (C) 2015 ArachisH

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License along
    with this program; if not, write to the Free Software Foundation, Inc.,
    51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA.

    See License.txt in the project root for license information.
*/

namespace Sulakore.Habbo.Web
{
    public class HGameData
    {
        public int Port { get; }
        public string Host { get; }
        public string Texts { get; }
        public int AccountId { get; }
        public string UniqueId { get; }
        public string Variables { get; }
        public string OverrideTexts { get; }
        public string ClientStarting { get; }
        public string FlashClientUrl { get; }
        public string FigurePartList { get; }
        public string FlashClientBuild { get; }
        public string FurniDataLoadUrl { get; }
        public string OverrideVariables { get; }
        public string ProductDataLoadUrl { get; }

        public HGameData(string gameData)
        {
            gameData = gameData.Replace("\\/", "/").Replace("\"//", "\"http://")
                .Replace("'//", "'http://");

            string flashVars = gameData.GetChild("var flashvars = {", '}')
                .Replace("\"", string.Empty).Replace(" : ", ":");

            string[] lines = flashVars.Split(',');
            foreach (string pair in lines)
            {
                string varName = pair.Split(':')[0].Trim();
                string varValue = pair.GetChild(varName + ":");

                #region Switch Statement: varName
                switch (varName)
                {
                    case "unique_habbo_id": UniqueId = varValue; break;
                    case "external.texts.txt": Texts = varValue; break;
                    case "connection.info.host": Host = varValue; break;
                    case "client.starting": ClientStarting = varValue; break;
                    case "account_id": AccountId = int.Parse(varValue); break;
                    case "external.variables.txt": Variables = varValue; break;
                    case "furnidata.load.url": FurniDataLoadUrl = varValue; break;
                    case "connection.info.port": Port = int.Parse(varValue); break;
                    case "productdata.load.url": ProductDataLoadUrl = varValue; break;
                    case "external.override.texts.txt": OverrideTexts = varValue; break;
                    case "external.figurepartlist.txt": FigurePartList = varValue; break;
                    case "external.override.variables.txt": OverrideVariables = varValue; break;
                    case "flash.client.url":
                    {
                        char valueEnd = '"';
                        string clientUrl = null;
                        int clientUrlIndex = gameData.IndexOf("embedSWF(");
                        if (clientUrlIndex != -1)
                        {
                            clientUrlIndex += 8;
                            valueEnd = gameData[clientUrlIndex + 1];
                            bool isVariable = (valueEnd != '"' && valueEnd != '\'');
                            if (isVariable)
                            {
                                clientUrl = gameData.GetChild(string.Format("{0} = \"",
                                    gameData.GetChild("embedSWF(", ',')), '"');
                            }
                            else clientUrl = gameData.GetChild("embedSWF(" + valueEnd, valueEnd);
                            clientUrl = clientUrl.Split('?')[0];
                        }
                        FlashClientUrl = clientUrl ?? "http:" + varValue + "Habbo.swf";

                        string[] segments = FlashClientUrl.Split('/');
                        FlashClientBuild = segments[segments.Length - 2];
                        break;
                    }
                }
                #endregion
            }
        }
    }
}