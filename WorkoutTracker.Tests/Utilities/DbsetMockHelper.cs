using FakeItEasy;
using Microsoft.EntityFrameworkCore;

namespace WorkoutTracker.Tests.Utilities;

public static class DbsetMockHelper
{
    public static DbSet<T> CreateMockDbSet<T>(List<T> data) where T : class
    {
        var queryableData = data.AsQueryable();

        var dbSetMock = A.Fake<DbSet<T>>(builder =>
            builder.Implements<IQueryable<T>>().Implements<IEnumerable<T>>());

        A.CallTo(() => ((IQueryable)dbSetMock).Provider).Returns(queryableData.Provider);
        A.CallTo(() => ((IQueryable)dbSetMock).Expression).Returns(queryableData.Expression);
        A.CallTo(() => ((IQueryable)dbSetMock).ElementType).Returns(queryableData.ElementType);
        A.CallTo(() => ((IQueryable)dbSetMock).GetEnumerator()).Returns(queryableData.GetEnumerator());

        return dbSetMock;
    }
}