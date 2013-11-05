using FormValidation.UILogic.Models;
using Microsoft.Practices.Prism.PubSubEvents;

namespace FormValidation.UILogic.Events
{
    public class PersonDeletedEvent : PubSubEvent<Person>
    {
    }
}
