﻿/*
Copyright (c) 2016 Radu

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

/*
DISCLAIMER
The reposiotory, code and tools provided herein are for educational purposes only.
The information not meant to change or impact the original code, product or service.
Use of this repository, code or tools does not exempt the user from any EULA, ToS or any other legal agreements that have been agreed with other parties.
The user of this repository, code and tools is responsible for his own actions.

Any forks, clones or copies of this repository are the responsability of their respective authors and users.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssetStudio
{
    class unityFont
    {
        public string m_Name;
        public byte[] m_FontData;

        public unityFont(AssetPreloadData preloadData, bool readSwitch)
        {
            var sourceFile = preloadData.sourceFile;
            var a_Stream = preloadData.sourceFile.a_Stream;
            a_Stream.Position = preloadData.Offset;

            if (sourceFile.platform == -2)
            {
                uint m_ObjectHideFlags = a_Stream.ReadUInt32();
                PPtr m_PrefabParentObject = sourceFile.ReadPPtr();
                PPtr m_PrefabInternal = sourceFile.ReadPPtr();
            }

            m_Name = a_Stream.ReadAlignedString(a_Stream.ReadInt32());

            if (readSwitch)
            {
                int m_AsciiStartOffset = a_Stream.ReadInt32();

                if (sourceFile.version[0] <= 3)
                {
                    int m_FontCountX = a_Stream.ReadInt32();
                    int m_FontCountY = a_Stream.ReadInt32();
                }

                float m_Kerning = a_Stream.ReadSingle();
                float m_LineSpacing = a_Stream.ReadSingle();

                if (sourceFile.version[0] <= 3)
                {
                    int m_PerCharacterKerning_size = a_Stream.ReadInt32();
                    for (int i = 0; i < m_PerCharacterKerning_size; i++)
                    {
                        int first = a_Stream.ReadInt32();
                        float second = a_Stream.ReadSingle();
                    }
                }
                else
                {
                    int m_CharacterSpacing = a_Stream.ReadInt32();
                    int m_CharacterPadding = a_Stream.ReadInt32();
                }

                int m_ConvertCase = a_Stream.ReadInt32();
                PPtr m_DefaultMaterial = sourceFile.ReadPPtr();

                int m_CharacterRects_size = a_Stream.ReadInt32();
                for (int i = 0; i < m_CharacterRects_size; i++)
                {
                    int index = a_Stream.ReadInt32();
                    //Rectf uv
                    float uvx = a_Stream.ReadSingle();
                    float uvy = a_Stream.ReadSingle();
                    float uvwidth = a_Stream.ReadSingle();
                    float uvheight = a_Stream.ReadSingle();
                    //Rectf vert
                    float vertx = a_Stream.ReadSingle();
                    float verty = a_Stream.ReadSingle();
                    float vertwidth = a_Stream.ReadSingle();
                    float vertheight = a_Stream.ReadSingle();
                    float width = a_Stream.ReadSingle();

                    if (sourceFile.version[0] >= 4)
                    {
                        bool flipped = a_Stream.ReadBoolean();
                        a_Stream.Position += 3;
                    }
                }

                PPtr m_Texture = sourceFile.ReadPPtr();

                int m_KerningValues_size = a_Stream.ReadInt32();
                for (int i = 0; i < m_KerningValues_size; i++)
                {
                    int pairfirst = a_Stream.ReadInt16();
                    int pairsecond = a_Stream.ReadInt16();
                    float second = a_Stream.ReadSingle();
                }

                if (sourceFile.version[0] <= 3)
                {
                    bool m_GridFont = a_Stream.ReadBoolean();
                    a_Stream.Position += 3; //4 byte alignment
                }
                else { float m_PixelScale = a_Stream.ReadSingle(); }

                int m_FontData_size = a_Stream.ReadInt32();
                if (m_FontData_size > 0)
                {
                    m_FontData = new byte[m_FontData_size];
                    a_Stream.Read(m_FontData, 0, m_FontData_size);

                    if (m_FontData[0] == 79 && m_FontData[1] == 84 && m_FontData[2] == 84 && m_FontData[3] == 79)
                    { preloadData.extension = ".otf"; }
                    else { preloadData.extension = ".ttf"; }

                }

                float m_FontSize = a_Stream.ReadSingle();//problem here in minifootball
                float m_Ascent = a_Stream.ReadSingle();
                uint m_DefaultStyle = a_Stream.ReadUInt32();

                int m_FontNames = a_Stream.ReadInt32();
                for (int i = 0; i < m_FontNames; i++)
                {
                    string m_FontName = a_Stream.ReadAlignedString(a_Stream.ReadInt32());
                }

                if (sourceFile.version[0] >= 4)
                {
                    int m_FallbackFonts = a_Stream.ReadInt32();
                    for (int i = 0; i < m_FallbackFonts; i++)
                    {
                        PPtr m_FallbackFont = sourceFile.ReadPPtr();
                    }

                    int m_FontRenderingMode = a_Stream.ReadInt32();
                }
            }
            else
            {
                if (m_Name != "") { preloadData.Text = m_Name; }
                else { preloadData.Text = preloadData.TypeString + " #" + preloadData.uniqueID; }
                preloadData.SubItems.AddRange(new string[] { preloadData.TypeString, preloadData.exportSize.ToString() });
            }
        }
    }
}
