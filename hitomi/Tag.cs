using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace hitomi
{
    public struct Tag
    {
        public readonly string Original;
        public readonly string Value;
        public readonly Sex Sex;

        public Tag(string tag)
        {
            Original = tag;

            if (tag.EndsWith("♀"))
                Sex = Sex.FEMALE;
            else if (tag.EndsWith("♂"))
                Sex = Sex.MALE;
            else
                Sex = Sex.NONE;

            Value = tag.TrimEnd(' ', '♀', '♂');
        }
    }

    public enum Sex
    {
        NONE,
        FEMALE,
        MALE
    }
}
