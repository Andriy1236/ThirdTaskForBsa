using System;
using System.Collections.Generic;
using System.Linq;

namespace AddressBookLibrary
{

    
    ////////////////////////Task3////////////////////////////
    public static class Utils
{
        public static IEnumerable<User> GetAdultUsersFromKiev(this List<User> users)
        {
            DateTime eighteenYearsAgo = DateTime.Today.AddYears(-18);
            var adultUsersFromKiev = users.Where(user => user.BirthDate < eighteenYearsAgo && user.City == "Киев");
            foreach (var user in adultUsersFromKiev)
            {
                yield return user;
            }
        }
}
    //////////////////////////////////////////////////////////////
   

    public class AddressBook
    {
        public delegate void EventHandler(string type, string log);

        public AddressBook()
        {
            _addressBook = new List<User>();
        }
        public event EventHandler UserAdded;
        public event EventHandler UserRemoved;
        private List<User> _addressBook;
        public bool AddUser(User user)
        {
            try
            {
                var isUserAdded = false;

                if (_addressBook == null)
                {
                    throw new NullReferenceException("Книга не створена! ");
                }
                if (user == null)
                {
                    throw new NullReferenceException("Немає користувача! ");
                }

                UserAdded?.Invoke("debug", "Почався запис користувача в адресну книгу");

                if (IsUserInAdressBook(user, _addressBook)) //провірка на наявність юзера в книзі
                {
                    UserAdded?.Invoke("warning", "користувач з такими данними вже є!");
                }
                if (!IsUserInAdressBook(user, _addressBook)) //добавиться тільки тоді , коли не співпадає 
                {
                    _addressBook.Add(user);
                    UserAdded?.Invoke("info", string.Format("Був доданий користувач " + user.FirstName + " " + user.LastName));
                    UserAdded?.Invoke("debug", "Закінчився запис користувача в адресну книгу");
                    isUserAdded = true;
                }
                return isUserAdded;
            }
            catch (NullReferenceException ex)
            {
                UserAdded?.Invoke("error", string.Format(ex.Message + "StackTrace :" + ex.StackTrace));
                return false;
            }
            catch (Exception ex)
            {
                UserAdded?.Invoke("error", string.Format(ex.Message + "StackTrace :" + ex.StackTrace));
                return false;
            }
        }

        public bool RemoveUser(User user)
        {
            try
            {
                bool isUserRemoved = false;

                if (_addressBook == null)
                {
                    throw new NullReferenceException("Книга не створена!");
                }
                if (user == null)
                {
                    throw new NullReferenceException("Немає користувача!");
                }
                UserRemoved?.Invoke("debug", "Почався процес видалення користувача з  адресної книги");

                if (!IsUserInAdressBook(user, _addressBook)) //видалиться тільки тоді , коли такий користувач є
                {
                    _addressBook.Remove(user);
                    UserRemoved?.Invoke("info", string.Format(" Був видалений користувач " + user.FirstName + " " + user.LastName));
                    UserRemoved?.Invoke("debug", " Закінчився процес видалення користувача з адресної книгу ");
                    isUserRemoved = true;
                }
                return isUserRemoved;
            }
            catch (NullReferenceException ex)
            {
                UserRemoved?.Invoke("error", string.Format(ex.Message + "StackTrace :" + ex.StackTrace));
                return false;
            }
            catch (Exception ex)
            {
                UserRemoved?.Invoke("error", string.Format(ex.Message + "StackTrace :" + ex.StackTrace));
                return false;
            }
        }


        private bool IsUserInAdressBook(User user, IEnumerable<User> listOfUsers)
        {
            bool isUserInList = false;
            foreach (var record in listOfUsers)
            {
                if (user.FirstName == record.FirstName && user.LastName == record.LastName && user.PhoneNumber == record.PhoneNumber)
                {
                    isUserInList = true;
                }
            }
            return isUserInList;
        }
        ////////////////////////Task3////////////////////////////
        public IEnumerable<User> FindUsersWithGmailEmail()
        {
            var usersWithGmailEmail = _addressBook.Where(user =>user.Email.Contains("gmail.com"));
            return usersWithGmailEmail;
        }
       public IEnumerable<User> FindLastTenDaysAddedGirls()
       {
           DateTime tenDaysAgo = DateTime.Today.AddDays(-10);
           var usersGirlsLastTenDays = from user in _addressBook
                                       where user.Gender == Gender.Female && user.TimeAdded >= tenDaysAgo
                                       select user;
           return usersGirlsLastTenDays;
       }

        public List<User> FindUsersWithBirthDayInJanuaryAndHaveNotEmptyAdressAndPhone()
        {
            var usersWithBirthDayInJanuarAndHaveNotEmptyAdressAndPhone =
                _addressBook.Where(user => user.BirthDate.Month == 1 && !string.IsNullOrEmpty(user.Addres) && !string.IsNullOrEmpty(user.PhoneNumber))
                .OrderByDescending(user => user.LastName).ToList();
            return usersWithBirthDayInJanuarAndHaveNotEmptyAdressAndPhone;
        }

        public Dictionary<string, List<User>> GetGenderDictionary()
        {
            Dictionary<string, List<User>> genderDictionary = new Dictionary<string, List<User>>();
            var listWithMans = _addressBook.Where(user => user.Gender == Gender.Male).ToList();
            var listWithWomans = _addressBook.Where(user => user.Gender == Gender.Female).ToList();
            genderDictionary.Add("man", listWithMans);
            genderDictionary.Add("woman", listWithWomans);
            return genderDictionary;
        }

        public int FindCountUsersFromCityWhoseBirthdayToday(string city, DateTime birthdate)
        {
            var countUsersFromCityWhoseBirthdayToday = (from user in _addressBook
                where user.City == city && user.BirthDate.ToShortDateString() == birthdate.ToShortDateString()
                select user).Count();
            return countUsersFromCityWhoseBirthdayToday;
        }


        public IEnumerable<User> GetUsersFromIntervalWithConditional(Func<User, bool> conditions, int first, int last)
        {
            var plural = _addressBook.Skip(first - 1).Take(last - first + 1).Where(user => conditions(user)); // мінус 1, бо початок є з 0 елемента.
            return plural;
        }

        public IEnumerable<User> FindAdultUsersFromKiev()
        {
            var  adultUsersFromKiev = _addressBook.GetAdultUsersFromKiev();
            return adultUsersFromKiev;
        }
    }
}
