using Moe.La.Core.Entities;
using Moe.La.UnitTests.Builders;
using System;
using System.Collections.Generic;
using Xunit;

namespace Moe.La.UnitTests
{
    public class BoardMeetingUnitTest : BaseUnitTest
    {
        [Fact]
        public async void Get_ALL_Meetings_With_Valid_Information()
        {
            // Arrange
            var legalBoardService = ServiceHelper.CreateLegalBoardService();
            var legalBoardDto = new LegalBoardBuilder().WithDefaultValues().Build();
            var legalBoard = await legalBoardService.AddAsync(legalBoardDto);

            var legalMemoService = ServiceHelper.CreateLegalMemoService();
            var legalMemoDto = new LegalMemoBuilder().WithDefaultValues().Build();
            var legalMemo = await legalMemoService.AddAsync(legalMemoDto);

            var boardMeetingDto = new BoardMeetingBuilder().WithDefaultValues()
                .BoardId(legalBoard.Data.Id)
                .MemoId(legalMemo.Data.Id)
                .BoardMeetingMembers(new List<int>() { 1, 2 })
                .Build();

            // Act 
            await legalBoardService.AddBoardMeetingAsync(boardMeetingDto);
            var result = await legalBoardService.GetAllBoardMeetingAsync(new BoardMeetingQueryObject());

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Data.TotalItems);
            Assert.NotEmpty(result.Data.Items);
        }

        [Fact]
        public async void Get_Meeting_With_Valid_Information()
        {
            // Arrange
            var legalBoardService = ServiceHelper.CreateLegalBoardService();
            var legalBoardDto = new LegalBoardBuilder().WithDefaultValues().Build();
            var legalBoard = await legalBoardService.AddAsync(legalBoardDto);

            var legalMemoService = ServiceHelper.CreateLegalMemoService();
            var legalMemoDto = new LegalMemoBuilder().WithDefaultValues().Build();
            var legalMemo = await legalMemoService.AddAsync(legalMemoDto);

            var boardMeetingDto = new BoardMeetingBuilder().WithDefaultValues()
                .BoardId(legalBoard.Data.Id)
                .MemoId(legalMemo.Data.Id)
                .BoardMeetingMembers(new List<int>() { 1, 2 })
                .Build();

            // Act 
            await legalBoardService.AddBoardMeetingAsync(boardMeetingDto);
            var result = await legalBoardService.GetBoardMeetingAsync((int)boardMeetingDto.Id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
            Assert.Equal(boardMeetingDto.MeetingPlace, result.Data.MeetingPlace);
            Assert.NotEqual(DateTime.Now, result.Data.MeetingDate);
            Assert.Equal(boardMeetingDto.BoardMeetingMembers.Count, result.Data.BoardMeetingMembersIds.Count);
            Assert.Equal(legalBoardDto.Name, result.Data.Board);
            Assert.Equal(legalMemoDto.Name, result.Data.Memo.Name);
        }

        [Fact]
        public async void Add_New_Meeting_With_Valid_Information()
        {
            // Arrange
            var legalBoardService = ServiceHelper.CreateLegalBoardService();
            var legalBoardDto = new LegalBoardBuilder().WithDefaultValues().Build();
            var legalBoard = await legalBoardService.AddAsync(legalBoardDto);

            var legalMemoService = ServiceHelper.CreateLegalMemoService();
            var legalMemoDto = new LegalMemoBuilder().WithDefaultValues().Build();
            var legalMemo = await legalMemoService.AddAsync(legalMemoDto);

            var boardMeetingDto = new BoardMeetingBuilder().WithDefaultValues()
                .BoardId(legalBoard.Data.Id)
                .MemoId(legalMemo.Data.Id)
                .BoardMeetingMembers(new List<int>() { 1, 2 })
                .Build();

            // Act 
            var result = await legalBoardService.AddBoardMeetingAsync(boardMeetingDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True(result.Data.Id > 0);
            Assert.Equal(boardMeetingDto.MeetingPlace, result.Data.MeetingPlace);
            Assert.NotEqual(DateTime.Now, result.Data.MeetingDate);
            Assert.Equal(boardMeetingDto.BoardMeetingMembers.Count, result.Data.BoardMeetingMembers.Count);
            Assert.Equal(legalBoardDto.Id, result.Data.BoardId);
            Assert.Equal(legalMemoDto.Id, result.Data.MemoId);
        }

        [Fact]
        public async void Edit_Meeting_With_Valid_Information()
        {
            // Arrange
            var legalBoardService = ServiceHelper.CreateLegalBoardService();
            var legalBoardDto = new LegalBoardBuilder().WithDefaultValues().Build();
            var legalBoard = await legalBoardService.AddAsync(legalBoardDto);

            var legalMemoService = ServiceHelper.CreateLegalMemoService();
            var legalMemoDto = new LegalMemoBuilder().WithDefaultValues().Build();
            var legalMemo = await legalMemoService.AddAsync(legalMemoDto);

            var boardMeetingDto = new BoardMeetingBuilder().WithDefaultValues()
                .BoardId(legalBoard.Data.Id)
                .MemoId(legalMemo.Data.Id)
                .BoardMeetingMembers(new List<int>() { 1, 2 })
                .Build();

            // Act 
            await legalBoardService.AddBoardMeetingAsync(boardMeetingDto);
            boardMeetingDto.MeetingPlace = "Smart Fingers";
            boardMeetingDto.MeetingDate = new DateTime(2020, 1, 1);
            boardMeetingDto.BoardMeetingMembers = new List<int>() { 1 };
            var result = await legalBoardService.EditBoardMeetingAsync(boardMeetingDto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Smart Fingers", result.Data.MeetingPlace);
            Assert.Equal(new DateTime(2020, 1, 1), result.Data.MeetingDate);
            Assert.Equal(new List<int>() { 1 }, result.Data.BoardMeetingMembers);
        }
    }
}
