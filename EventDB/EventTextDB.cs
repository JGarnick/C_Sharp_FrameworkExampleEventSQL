using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventPropsClasses;
using ToolsCSharp;

using DBBase = ToolsCSharp.BaseTextDB;

namespace EventDBClasses
{
    public class EventTextDB : DBBase, IReadDB, IWriteDB
    {
        #region Constructors

        public EventTextDB() : base() { }
        public EventTextDB(string cnString) : base(cnString) { }

        #endregion

        public int IndexOf(List<EventProps> list, int key)
        {
            int index = 0;
            foreach (EventProps props in list)
            {
                if (props.ID == key)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        public int NextID(List<EventProps> list)
        {
            int next = Int32.MinValue;
            foreach (EventProps props in list)
                if (props.ID > next)
                    next = props.ID;
            next++;
            return next;
        }

        public void Delete(int id)
        {
            EventProps props = new EventProps();
            props.ID = id;
            Delete(props);
        }

        #region IReadDB Members
        /// <summary>
        /// </summary>
        public IBaseProps Retrieve(Object key)
        {
            EventProps props;
            List<EventProps> events = new List<EventProps>();

            try
            {
                events = (List<EventProps>)RetrieveAll(events.GetType());
                int index = IndexOf(events, (int)key);
                if (index != -1)
                {
                    props = (EventProps)events[index];
                    return props;
                }
                else
                    throw new Exception("Event with id of " + key.ToString() + " does not exist");
            }

            catch (Exception e)
            {
                // log this exception
                throw;
            }

            finally
            {
            }
        } // end of Retrieve()
        #endregion

        #region IWriteDB Members
        /// <summary>
        /// </summary>
        public IBaseProps Create(IBaseProps p)
        {
            EventProps props = (EventProps)p;
            List<EventProps> events = new List<EventProps>();

            try
            {
                events = (List<EventProps>)RetrieveAll(events.GetType());
                props.ID = NextID(events);
                props.ConcurrencyID = 1;
                events.Add(props);
                WriteAll(events);
                return props;
            }

            catch (Exception e)
            {
                // log the error
                throw;
            }

            finally
            {
            }
        } // end of Create()

        /// <summary>
        /// </summary>
        public bool Delete(IBaseProps p)
        {
            EventProps props = (EventProps)p;
            List<EventProps> events = new List<EventProps>();

            try
            {
                events = (List<EventProps>)RetrieveAll(events.GetType());
                int index = IndexOf(events, props.ID);
                if (index != -1)
                {
                    events.RemoveAt(index);
                    WriteAll(events);
                    return true;
                }
                else
                    throw new Exception("Event with id of " + props.ID.ToString() + " does not exist");

            }

            catch (Exception e)
            {
                // log the error
                throw;
            }

            finally
            {
            }
        } // end of Delete()

        /// <summary>
        /// </summary>
        public bool Update(IBaseProps p)
        {
            EventProps props = (EventProps)p;
            List<EventProps> events = new List<EventProps>();

            try
            {
                events = (List<EventProps>)RetrieveAll(events.GetType());
                int index = IndexOf(events, props.ID);
                if (index != -1)
                {
                    if (props.ConcurrencyID == events[index].ConcurrencyID)
                    {
                        events.RemoveAt(index);
                        props.ConcurrencyID++;
                        events.Add(props);
                        WriteAll(events);
                        return true;
                    }
                    else
                        throw new Exception("Event with id of " + props.ID.ToString() + " appears to have been edited by another user.  Changes can not be saved.");
                }
                else
                    throw new Exception("Event with id of " + props.ID.ToString() + " does not exist");

            }

            catch (Exception e)
            {
                // log this error
                throw;
            }

            finally
            {
            }
        } // end of Update()
        #endregion
    }
}
