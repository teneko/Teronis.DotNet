using System;
using System.Linq;
using Teronis.Extensions.NetStandard;

namespace Teronis.Text.UriBuilder
{
    public abstract class PathAndQueryBuilder
    {
        private static readonly Type LocalType = typeof(PathAndQueryBuilder);

        static readonly string CustomBrackets = "()";
        static readonly string PathKeySeparator = "/?";

        public abstract string PathAndQueryTemplate { get; }

        public string KVPsConst { get { return ValInCustomBrackets("KVPs"); } }

        public PathAndQueryBuilder() { }

        public PathAndQueryBuilder(string uri)
        {
            var firstSlashIndex = uri.IndexOf('/', 0);
            var firstQuestionMarkIndex = uri.IndexOf('?', 0);
            var pathAndQuery = uri;

            var replaces = new Tuple<string, string>[] {
                new Tuple<string, string>("%7B", "{"),
                new Tuple<string, string>("%7D", "}"),
                new Tuple<string, string>("%5B", "["),
                new Tuple<string, string>("%5D", "]")
            };

            foreach (var tuple in replaces)
                pathAndQuery = pathAndQuery.Replace(tuple.Item1, tuple.Item2);

            var pathValueMembers = GetType().GetAttributePropertyMembers<LinkPathValueAttribute>().ToArray();
            //var pathValueMembers = getMembers<LinkPathValueAttribute>().ToArray();
            var localFileAndQueryEnumerator = PathAndQueryTemplate.GetEnumerator();
            var fileAndQueryEnumerator = pathAndQuery.GetEnumerator();

            while (localFileAndQueryEnumerator.MoveNext() && fileAndQueryEnumerator.MoveNext() && fileAndQueryEnumerator.Current != '?') {
                if (localFileAndQueryEnumerator.Current == fileAndQueryEnumerator.Current)
                    continue;
                else {
                    string key = "";

                    while (localFileAndQueryEnumerator.MoveNext() && localFileAndQueryEnumerator.Current != CustomBrackets[1]) {
                        key += localFileAndQueryEnumerator.Current;
                    }

                    localFileAndQueryEnumerator.MoveNext();
                    string value = "";

                    do {
                        value += fileAndQueryEnumerator.Current;
                    } while (fileAndQueryEnumerator.MoveNext() && !PathKeySeparator.Any(x => x == fileAndQueryEnumerator.Current));

                    var member = pathValueMembers.Single(x => { return (x.Attributes.Single().Name ?? x.MemberInfo.Name) == key; }); //Console.WriteLine($"x_attr_name:{x.Attribute.Name} x_name:{x.Name} key:{key}"); 
                    member.MemberInfo.SetValue(null,Convert.ChangeType(value, member.MemberInfo.GetVariableType()));
                }
            }

            var keyValueMembers = LocalType.GetAttributeVariableMembers<LinkKeyValueAttribute>().ToArray();

            if (keyValueMembers.Length != 0 && fileAndQueryEnumerator.Current == '?') {
                fileAndQueryEnumerator.MoveNext();

                do {
                    string key = "";

                    do {
                        if (fileAndQueryEnumerator.Current == '=')
                            break;

                        key += fileAndQueryEnumerator.Current;
                    } while (fileAndQueryEnumerator.MoveNext());

                    string value = "";
                    bool endable = true;
                    int openerCounter = 0;

                    while (fileAndQueryEnumerator.MoveNext() && !(endable && fileAndQueryEnumerator.Current == '&')) {
                        value += fileAndQueryEnumerator.Current;

                        if ("[{".Any(x => x == fileAndQueryEnumerator.Current))
                            openerCounter++;
                        else if ("]}".Any(x => x == fileAndQueryEnumerator.Current))
                            openerCounter--;
                    }

                    if (value == ValInBrackets(key, "{}") || key.Contains(ValWithoutBrackets(value).UppercaseFirstLetter())) // pageSize={size}
                        continue;
                    else {
                        var member = keyValueMembers.Single(x => { return (x.Attributes.Single().Name ?? x.MemberInfo.Name) == key; }); //Console.WriteLine($"x_attr_name:{x.Attribute.Name} x_name:{x.Name} key:{key}"); 
                        member.MemberInfo.SetValue(null, Convert.ChangeType(value, member.MemberInfo.GetVariableType()));
                    }
                } while (fileAndQueryEnumerator.MoveNext());
            }
        }

        public string ValInCustomBrackets(object val)
        {
            return ValInBrackets(val, CustomBrackets);
        }

        public string ValInBrackets(object val, string brackets)
        {
            return $"{brackets[0]}{val}{brackets[1]}";
        }

        public string ValWithoutBrackets(string val)
        {
            return val.Trim(new char[] { '{', '}', '[', ']' });
        }

        string getKeyValuePart()
        {
            var retVal = (string)null;

            var members = LocalType.GetAttributeVariableMembers<LinkKeyValueAttribute>().Where(x => {
                var attribute = x.Attributes.Single();

                return !attribute.IsDefaultSet || (attribute.IsDefaultSet && !attribute.ProofValueEquality(x.MemberInfo.GetValue(null)));
            }).ToList();

            for (int index = 0; index < members.Count; index++) {
                var member = members[index];
                var attribute = member.Attributes.Single();
                var propertyValue = member.MemberInfo.GetValue(null);

                retVal += $"{attribute.Name ?? member.MemberInfo.Name}={propertyValue}";

                if (index < members.Count - 1)
                    retVal += "&";
            }

            if (retVal != null)
                return "?" + retVal;
            //
            return null;
        }

        public string BuildPathAndQuery()
        {
            var retVal = PathAndQueryTemplate;
            var pathValueMembers = LocalType.GetAttributeVariableMembers<LinkPathValueAttribute>().ToArray();

            if (pathValueMembers.Length != 0) {
                foreach (var member in pathValueMembers) {
                    var key = ValInCustomBrackets(member.Attributes.Single().Name ?? member.MemberInfo.Name);
                    retVal = retVal.Replace(key, member.MemberInfo.GetValue(null).ToString());
                }
            }

            if (retVal.Contains(KVPsConst))
                retVal = retVal.Replace(KVPsConst, getKeyValuePart());

            return retVal;
        }

        public string BuildUrl(string domain)
            => new Uri(new Uri(domain), BuildPathAndQuery()).AbsoluteUri;
    }
}
