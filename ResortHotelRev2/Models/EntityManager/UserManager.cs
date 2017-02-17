using ResortHotelRev2.Models.DB;
using ResortHotelRev2.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ResortHotelRev2.Models.EntityManager
{
    public class UserManager
    {
        public void AddUserAccount(UserSignUpView user)
        {

            ResortDBEntities db = new ResortDBEntities();
            {

                SYSUser sysUser = new SYSUser();
                sysUser.LoginName = user.LoginName;
                sysUser.PasswordEncryptedText = user.Password;
                sysUser.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                sysUser.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1; ;
                sysUser.RowCreatedDateTime = DateTime.Now;
                sysUser.RowMOdifiedDateTime = DateTime.Now;

                db.SYSUsers.Add(sysUser);
                db.SaveChanges();

                SYSUserProfile sysUserProfile = new SYSUserProfile();
                sysUserProfile.SYSUserID = sysUser.SYSUserID;
                sysUserProfile.FirstName = user.FirstName;
                sysUserProfile.LastName = user.LastName;
                sysUserProfile.PhoneNumber = user.PhoneNumber;
                sysUserProfile.Email = user.Email;
                sysUserProfile.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                sysUserProfile.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                sysUserProfile.RowCreatedDateTime = DateTime.Now;
                sysUserProfile.RowModifiedDateTime = DateTime.Now;
                

                db.SYSUserProfiles.Add(sysUserProfile);
                db.SaveChanges();

                SYSUserRole sysUserRole = new SYSUserRole();
                sysUserRole.LOOKUPRoleID = 2;
                sysUserRole.SYSUserID = sysUser.SYSUserID;
                sysUserRole.IsActive = true;
                sysUserRole.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                sysUserRole.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                sysUserRole.RowCreatedDateTime = DateTime.Now;
                sysUserRole.RowModifiedDateTime = DateTime.Now;

                db.SYSUserRoles.Add(sysUserRole);
                db.SaveChanges();
                

            }
        }

        public bool IsLoginNameExist(string loginName)
        {
            ResortDBEntities db = new ResortDBEntities();
            {
                return db.SYSUsers.Where(o => o.LoginName.Equals(loginName)).Any();
            }
        }

        public string GetUserPassword(string loginName)
        {
            ResortDBEntities db = new ResortDBEntities();
            {
                var user = db.SYSUsers.Where(o => o.LoginName.ToLower().Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().PasswordEncryptedText;
                else
                    return string.Empty;
            }
        }

        public bool IsUserInRole(string loginName, string roleName)
        {
            ResortDBEntities db = new ResortDBEntities();
            {
                SYSUser sysUser = db.SYSUsers.Where(o => o.LoginName.ToLower().Equals(loginName))?.FirstOrDefault();
                if (sysUser != null)
                {
                    var roles = from q in db.SYSUserRoles
                                join r in db.LOOKUPRoles on q.LOOKUPRoleID equals r.LOOKUPRoleID
                                where r.RoleName.Equals(roleName) && q.SYSUserID.Equals(sysUser.SYSUserID)
                                select r.RoleName;

                    if (roles != null)
                    {
                        return roles.Any();
                    }
                }

                return false;
            }
        }

        public List<LOOKUPAvailableRole> GetAllRoles()
        {
            ResortDBEntities db = new ResortDBEntities();
            {
                var roles = db.LOOKUPRoles.Select(o => new LOOKUPAvailableRole
                {
                    LOOKUPRoleID = o.LOOKUPRoleID,
                    RoleName = o.RoleName,
                    RoleDescription = o.RoleDescription
                }).ToList();

                return roles;
            }
        }

        public int GetUserID(string loginName)
        {
            ResortDBEntities db = new ResortDBEntities();
            {
                var user = db.SYSUsers.Where(o => o.LoginName.Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().SYSUserID;
            }
            return 0;
        }
        public List<UserProfileView> GetAllUserProfiles()
        {
            List<UserProfileView> profiles = new List<UserProfileView>();
            ResortDBEntities db = new ResortDBEntities();
            {
                UserProfileView userProfileView;
                var users = db.SYSUsers.ToList();

                foreach (SYSUser u in db.SYSUsers)
                {
                    userProfileView = new UserProfileView();
                    userProfileView.SYSUserID = u.SYSUserID;
                    userProfileView.LoginName = u.LoginName;
                    userProfileView.Password = u.PasswordEncryptedText;

                    var systemUserProfile = db.SYSUserProfiles.Find(u.SYSUserID);
                    if (systemUserProfile != null)
                    {
                        userProfileView.FirstName = systemUserProfile.FirstName;
                        userProfileView.LastName = systemUserProfile.LastName;
                        userProfileView.PhoneNumber = systemUserProfile.PhoneNumber;
                        userProfileView.Email = systemUserProfile.Email;
                        userProfileView.UserNotes = systemUserProfile.UserNotes;
                    }

                    var SUR = db.SYSUserRoles.Where(o => o.SYSUserID.Equals(u.SYSUserID));
                    if (SUR.Any())
                    {
                        var userRole = SUR.FirstOrDefault();
                        userProfileView.LOOKUPRoleID = userRole.LOOKUPRoleID;
                        userProfileView.RoleName = userRole.LOOKUPRole.RoleName;
                        userProfileView.IsRoleActive = userRole.IsActive;
                    }

                    profiles.Add(userProfileView);
                }
            }

            return profiles;
        }

        public UserDataView GetUserDataView(string loginName)
        {
            UserDataView userDataView = new UserDataView();
            List<UserProfileView> profiles = GetAllUserProfiles();
            List<LOOKUPAvailableRole> roles = GetAllRoles();

            int? userAssignedRoleID = 0, userID = 0;

            userID = GetUserID(loginName);
            ResortDBEntities db = new ResortDBEntities();
            {
                userAssignedRoleID = db.SYSUserRoles.Where(o => o.SYSUserID == userID)?.FirstOrDefault().LOOKUPRoleID;

            }

            userDataView.UserProfile = profiles;
            userDataView.UserRoles = new UserRoles { SelectedRoleID = userAssignedRoleID, UserRoleList = roles };
            return userDataView;
        }
        public UserProfileView GetUserProfile(int userID)
        {
            UserProfileView userProfileView = new UserProfileView();
            ResortDBEntities db = new ResortDBEntities();
            {
                var user = db.SYSUsers.Find(userID);
                if (user != null)
                {
                    userProfileView.SYSUserID = user.SYSUserID;
                    userProfileView.LoginName = user.LoginName;
                    userProfileView.Password = user.PasswordEncryptedText;

                    var sysUserProfile = db.SYSUserProfiles.Find(userID);
                    if (sysUserProfile != null)
                    {
                        userProfileView.FirstName = sysUserProfile.FirstName;
                        userProfileView.LastName = sysUserProfile.LastName;
                        userProfileView.PhoneNumber = sysUserProfile.PhoneNumber;
                        userProfileView.Email = sysUserProfile.Email;
                        userProfileView.UserNotes = sysUserProfile.UserNotes;
                    }

                    var SUR = db.SYSUserRoles.Find(userID);
                    if (SUR != null)
                    {
                        userProfileView.LOOKUPRoleID = SUR.LOOKUPRoleID;
                        userProfileView.RoleName = SUR.LOOKUPRole.RoleName;
                        userProfileView.IsRoleActive = SUR.IsActive;
                    }
                }
            }
            return userProfileView;
        }

        public void UpdateUserAccount(UserProfileView user)
        {

            ResortDBEntities db = new ResortDBEntities();
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        SYSUser sysUser = db.SYSUsers.Find(user.SYSUserID);
                        sysUser.LoginName = user.LoginName;
                        sysUser.PasswordEncryptedText = user.Password;
                        sysUser.RowCreatedSYSUserID = user.SYSUserID;
                        sysUser.RowModifiedSYSUserID = user.SYSUserID;
                        sysUser.RowCreatedDateTime = DateTime.Now;
                        sysUser.RowMOdifiedDateTime = DateTime.Now;

                        db.SaveChanges();

                        var userProfile = db.SYSUserProfiles.Where(o => o.SYSUserID == user.SYSUserID);
                        if (userProfile.Any())
                        {
                            SYSUserProfile sysUserProfile = userProfile.FirstOrDefault();
                            sysUserProfile.SYSUserID = sysUser.SYSUserID;
                            sysUserProfile.FirstName = user.FirstName;
                            sysUserProfile.LastName = user.LastName;
                            sysUserProfile.PhoneNumber = user.PhoneNumber;
                            sysUserProfile.Email = user.Email;
                            sysUserProfile.UserNotes = user.UserNotes;
                            sysUserProfile.RowCreatedSYSUserID = user.SYSUserID;
                            sysUserProfile.RowModifiedSYSUserID = user.SYSUserID;
                            sysUserProfile.RowCreatedDateTime = DateTime.Now;
                            sysUserProfile.RowModifiedDateTime = DateTime.Now;

                            db.SaveChanges();
                        }

                        if (user.LOOKUPRoleID > 0)
                        {
                            var userRole = db.SYSUserRoles.Where(o => o.SYSUserID == user.SYSUserID);
                            SYSUserRole sysUserRole = null;
                            if (userRole.Any())
                            {
                                sysUserRole = userRole.FirstOrDefault();
                                sysUserRole.LOOKUPRoleID = user.LOOKUPRoleID;
                                sysUserRole.SYSUserID = user.SYSUserID;
                                sysUserRole.IsActive = true;
                                sysUserRole.RowCreatedSYSUserID = user.SYSUserID;
                                sysUserRole.RowModifiedSYSUserID = user.SYSUserID;
                                sysUserRole.RowCreatedDateTime = DateTime.Now;
                                sysUserRole.RowModifiedDateTime = DateTime.Now;
                            }
                            else
                            {
                                sysUserRole = new SYSUserRole();
                                sysUserRole.LOOKUPRoleID = user.LOOKUPRoleID;
                                sysUserRole.SYSUserID = user.SYSUserID;
                                sysUserRole.IsActive = true;
                                sysUserRole.RowCreatedSYSUserID = user.SYSUserID;
                                sysUserRole.RowModifiedSYSUserID = user.SYSUserID;
                                sysUserRole.RowCreatedDateTime = DateTime.Now;
                                sysUserRole.RowModifiedDateTime = DateTime.Now;
                                db.SYSUserRoles.Add(sysUserRole);
                            }

                            db.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

        public void DeleteUser(int userID)
        {
            ResortDBEntities db = new ResortDBEntities();
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {

                        var sysUserRole = db.SYSUserRoles.Where(o => o.SYSUserID == userID);
                        if (sysUserRole.Any())
                        {
                            db.SYSUserRoles.Remove(sysUserRole.FirstOrDefault());
                            db.SaveChanges();
                        }

                        var sysUserProfile = db.SYSUserProfiles.Where(o => o.SYSUserID == userID);
                        if (sysUserProfile.Any())
                        {
                            db.SYSUserProfiles.Remove(sysUserProfile.FirstOrDefault());
                            db.SaveChanges();
                        }

                        var SU = db.SYSUsers.Where(o => o.SYSUserID == userID);
                        if (SU.Any())
                        {
                            db.SYSUsers.Remove(SU.FirstOrDefault());
                            db.SaveChanges();
                        }

                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

    }


}