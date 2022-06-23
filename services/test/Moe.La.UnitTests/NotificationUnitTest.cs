using Moe.La.Core.Entities;
using Moe.La.UnitTests.Builders;
using Xunit;

namespace Moe.La.UnitTests
{
    public class NotificationUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Create_New_Notification_Given_Valid_Information()
        {
            // Arrange
            var notification = new NotificationBuilder().WithDefaultValues().Build();
            var notificationService = ServiceHelper.CreateNotificationService();

            // Act
            var result = await notificationService.AddAsync(notification);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
        }

        [Fact]
        public async void Get_Notification_System_List_Given_Valid_Search_Information()
        {
            // Arrange
            var notification = new NotificationBuilder().WithDefaultValues().Build();
            var notificationSystemService = ServiceHelper.CreateNotificationSystemService();
            var notificationService = ServiceHelper.CreateNotificationService();

            NotificationSystemQueryObject queryObject = new NotificationSystemQueryObject { IsForCurrentUser = true };

            // Act
            await notificationService.AddAsync(notification);
            var result = await notificationSystemService.GetAllAsync(queryObject);

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async void Get_Notification_System_By_Id_Given_Valid_Information()
        {
            // Arrange
            var notification = new NotificationBuilder().WithDefaultValues().Build();
            var notificationSystemService = ServiceHelper.CreateNotificationSystemService();
            var notificationService = ServiceHelper.CreateNotificationService();

            // Act
            await notificationService.AddAsync(notification);
            var result = await notificationSystemService.GetAsync(notification.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(notification.Id, result.Data.Id);
        }

        [Fact]
        public async void Read_Notification_System_Given_Valid_Information()
        {
            // Arrange
            var notification = new NotificationBuilder().WithDefaultValues().Build();
            var notificationSystemService = ServiceHelper.CreateNotificationSystemService();
            var notificationService = ServiceHelper.CreateNotificationService();

            // Act
            await notificationService.AddAsync(notification);
            var result = await notificationSystemService.ReadAsync(notification.Id);

            // Assert
            Assert.True(result.IsSuccess);
        }

        //[Fact]
        //public async void Get_Notification_Email_List_Given_Valid_Search_Information()
        //{
        //    // Arrange
        //    var notification = new NotificationBuilder().WithDefaultValues().Build();
        //    var notificationEmailService = ServiceHelper.CreateNotificationEmailService();
        //    var notificationService = ServiceHelper.CreateNotificationService();

        //    NotificationEmailQueryObject queryObject = new NotificationEmailQueryObject { IsForCurrentUser = true };

        //    // Act
        //    await notificationService.AddAsync(notification);
        //    var result = await notificationEmailService.GetAllAsync(queryObject);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //}

        //[Fact]
        //public async void Get_Notification_SMS_List_Given_Valid_Search_Information()
        //{
        //    // Arrange
        //    var notification = new NotificationBuilder().WithDefaultValues().Build();
        //    var notificationSMSService = ServiceHelper.CreateNotificationSMSService();
        //    var notificationService = ServiceHelper.CreateNotificationService();

        //    NotificationSMSQueryObject queryObject = new NotificationSMSQueryObject { IsForCurrentUser = true };

        //    // Act
        //    await notificationService.AddAsync(notification);
        //    var result = await notificationSMSService.GetAllAsync(queryObject);

        //    // Assert
        //    Assert.True(result.IsSuccess);
        //}
    }
}
