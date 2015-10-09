﻿using System;
using System.Text;

namespace Regexp
{
    public class ThompsonRegex
    {
        public ThompsonRegex ()
        {
        }
        private struct re2post_s{
            public int nalt;
            public int natom;
        }

        public static string re2post(string re)
        {
            int nalt, natom;
            StringBuilder buf=new StringBuilder();
            re2post_s[] paren = new re2post_s[100];//, *p;

            var p = 0;
            nalt = 0;
            natom = 0;
            if(re.Length >= 8000/2)
                return null;
            var re_ = re.ToCharArray ();
            for(int i=0; i<re_.Length; i++){
                switch( re_[i] ){
                case '(':
                    if(natom > 1){
                        --natom;
                        buf.Append('.');
                    }
                    if(p >= paren.Length+100)
                        return null;
                    paren[p].nalt = nalt;
                    paren[p].natom = natom;
                    p++;
                    nalt = 0;
                    natom = 0;
                    break;
                case '|':
                    if(natom == 0)
                        return null;
                    while(--natom > 0)
                        buf.Append('.');
                    nalt++;
                    break;
                case ')':
                    if(p == 0)
                        return null;
                    if(natom == 0)
                        return null;
                    while(--natom > 0)
                        buf.Append('.');
                    for(; nalt > 0; nalt--)
                        buf.Append('|');
                    --p;
                    nalt = paren[p].nalt;
                    natom = paren[p].natom;
                    natom++;
                    break;
                case '*':
                case '+':
                case '?':
                    if(natom == 0)
                        return null;
                    buf.Append(re_[i]);
                    break;
                default:
                    if(natom > 1){
                        --natom;
                        buf.Append( '.');
                    }
                    buf.Append( re_[i]);
                    natom++;
                    break;
                }
            }
            if(p != 0)
                return null;
            while(--natom > 0)
                buf.Append( '.');
            for(; nalt > 0; nalt--)
                buf.Append('|');
            //*dst = 0;
            return buf.ToString();
        }
    }
}
