
using System;
using System.Data;
using System.Collections.Generic;
using Lephone.Util;
using Lephone.Data.Driver;
using Lephone.Data.Definition;
using Lephone.Data.Builder;
using Lephone.Data.SqlEntry;

namespace Lephone.Data.Common
{
    internal abstract class EmitObjectHandlerBase : IDbObjectHandler
    {
        protected KeyValue[] kvc;

        public EmitObjectHandlerBase()
        {
        }

        protected KeyValue NewKeyValue(int n, object v)
        {
            KeyValue kv = kvc[n];
            return new KeyValue(kv.Key, v, kv.ValueType);
        }

        protected void AddKeyValue(KeyValueCollection Values, DbObjectSmartUpdate o, string Key, int n, object v)
        {
            Dictionary<string, object> kd = o.m_UpdateColumns;
            if (kd != null)
            {
                if (kd.ContainsKey(Key))
                {
                    Values.Add(NewKeyValue(n, v));
                }
            }
        }

        public void Init(ObjectInfo oi)
        {
            List<KeyValue> ret = new List<KeyValue>();
            foreach (MemberHandler fi in oi.Fields)
            {
                if (!fi.IsDbGenerate && !fi.IsHasOne && !fi.IsHasMany && !fi.IsHasAndBelongsToMany)
                {
                    ret.Add(GetKeyValue(fi));
                }
            }
            kvc = ret.ToArray();
        }

        private KeyValue GetKeyValue(MemberHandler fi)
        {
            if (fi.IsBelongsTo)
            {
                Type t = fi.FieldType.GetGenericArguments()[0];
                MemberHandler h = DbObjectHelper.GetKeyField(t);
                KeyValue kv = new KeyValue(fi.Name, null, h.FieldType);
                return kv;
            }
            else if (fi.IsLazyLoad)
            {
                Type t = fi.FieldType.GetGenericArguments()[0];
                KeyValue kv = new KeyValue(fi.Name, null, t);
                return kv;
            }
            else
            {
                KeyValue kv = new KeyValue(fi.Name, null, fi.FieldType);
                return kv;
            }
        }

        public abstract object CreateInstance();

        public void LoadSimpleValues(object o, bool UseIndex, IDataReader dr)
        {
            if (UseIndex)
            {
                LoadSimpleValuesByIndex(o, dr);
            }
            else
            {
                LoadSimpleValuesByName(o, dr);
            }
        }

        protected abstract void LoadSimpleValuesByIndex(object o, IDataReader dr);
        protected abstract void LoadSimpleValuesByName(object o, IDataReader dr);

        public void LoadRelationValues(DbContext driver, object o, bool UseIndex, IDataReader dr)
        {
            if (UseIndex)
            {
                LoadRelationValuesByIndex(driver, o, dr);
            }
            else
            {
                LoadRelationValuesByName(driver, o, dr);
            }
        }

        protected abstract void LoadRelationValuesByIndex(DbContext driver, object o, IDataReader dr);
        protected abstract void LoadRelationValuesByName(DbContext driver, object o, IDataReader dr);

        public Dictionary<string, object> GetKeyValues(object o)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            GetKeyValuesDirect(dic, o);
            return dic;
        }

        protected abstract void GetKeyValuesDirect(Dictionary<string, object> dic, object o);

        public object GetKeyValue(object o)
        {
            object ret = GetKeyValueDirect(o);
            if (ret == null)
            {
                throw new DbEntryException("The class must and just have one key");
            }
            return ret;
        }

        protected virtual object GetKeyValueDirect(object o) { return null; }

        public void SetValuesForSelect(ISqlKeys isv)
        {
            SetValuesForSelectDirect(isv.Keys);
        }

        protected abstract void SetValuesForSelectDirect(List<string> Keys);

        public void SetValuesForInsert(ISqlValues isv, object obj)
        {
            SetValuesForInsertDirect(isv.Values, obj);
        }

        protected abstract void SetValuesForInsertDirect(KeyValueCollection Values, object obj);

        public void SetValuesForUpdate(ISqlValues isv, object obj)
        {
            SetValuesForUpdateDirect(isv.Values, obj);
        }

        protected abstract void SetValuesForUpdateDirect(KeyValueCollection Values, object obj);

        protected object GetNullable(object o)
        {
            if (o == DBNull.Value)
            {
                return null;
            }
            return o;
        }
    }
    /*
    internal enum GenderType
    {
        Male,
        Female,
    }

    internal abstract class User : DbObjectModel<User>
    {
        [DbKey(IsDbGenerate=false)]
        public abstract int Key { get; set; }
        public abstract string Name { get; set; }
        public abstract int Age { get; set; }
        public abstract DateTime Time { get; set; }
        public abstract GenderType Gender { get; set; }
        public abstract bool IsLocal { get; set; }

        public HasMany<User> Children;
        public BelongsTo<User> Parent;

        public LazyLoadField<string> Profile;

        [AllowNull]
        public abstract string NullString { get; set; }

        public abstract int? NullInt { get; set; }
    }

    internal class UserValueHandler : EmitObjectHandlerBase
    {
        protected override void LoadSimpleValuesByIndex(object o, IDataReader dr)
        {
            User u = (User)o;
            u.m_Id = dr.GetInt64(0);
            u.Name = dr.GetString(1);
            u.Age = dr.GetInt32(2);
            u.Time = dr.GetDateTime(3);
            u.Gender = (GenderType)dr.GetInt32(4);
            u.IsLocal = dr.GetBoolean(5);
            u.NullString = (string)GetNullable(dr[6]);
            u.NullInt = (int?)GetNullable(dr[7]);
        }

        protected override void LoadSimpleValuesByName(object o, IDataReader dr)
        {
            User u = (User)o;
            u.m_Id = (long)dr["Id"];
            u.Name = (string)dr["Name"];
            u.Age = (int)dr["Age"];
            u.Time = (DateTime)dr["Time"];
            u.Gender = (GenderType)dr["Gender"];
            u.IsLocal = (bool)dr["IsLocal"];
            u.NullString = (string)GetNullable(dr["NullString"]);
            u.NullInt = (int?)GetNullable(dr["NullInt"]);
        }

        protected override void LoadRelationValuesByIndex(DbContext driver, object o, IDataReader dr)
        {
            User u = (User)o;
            ((ILazyLoading)u.Profile).Init(driver, "t1");
            ((ILazyLoading)u.Children).Init(driver, "t2");
            ((IBelongsTo)u.Parent).ForeignKey = dr[6];
        }

        protected override void LoadRelationValuesByName(DbContext driver, object o, IDataReader dr)
        {
            User u = (User)o;
            ((ILazyLoading)u.Profile).Init(driver, "t1");
            ((ILazyLoading)u.Children).Init(driver, "t2");
            ((IBelongsTo)u.Parent).ForeignKey = dr["Parent"];
        }

        protected override void GetKeyValuesDirect(Dictionary<string, object> dic, object o)
        {
            User u = (User)o;
            dic.Add("Id", u.m_Id);
            dic.Add("Key", u.Key);
        }

        protected override object GetKeyValueDirect(object o)
        {
            return ((User)o).m_Id;
        }

        public override object CreateInstance()
        {
            return User.New();
        }

        protected override void SetValuesForSelectDirect(List<string> Keys)
        {
            Keys.Add("Id");
            Keys.Add("Name");
            Keys.Add("Age");
            Keys.Add("Time");
            Keys.Add("Gender");
            Keys.Add("IsLocal");
        }

        protected override void SetValuesForInsertDirect(KeyValueCollection Values, object obj)
        {
            User u = (User)obj;
            Values.Add(NewKeyValue(0, u.Name));
            Values.Add(NewKeyValue(1, u.Age));
            Values.Add(NewKeyValue(2, u.Parent.ForeignKey));
            Values.Add(NewKeyValue(3, u.Profile.Value));
        }

        protected override void SetValuesForUpdateDirect(KeyValueCollection Values, object obj)
        {
            User u = (User)obj;
            AddKeyValue(Values, u, "Name", 0, u.Name);
            AddKeyValue(Values, u, "Age", 1, u.Age);
            AddKeyValue(Values, u, "$", 2, u.Parent.ForeignKey);
            AddKeyValue(Values, u, "Profile", 3, u.Profile.Value);
        }
    }
     */
}
