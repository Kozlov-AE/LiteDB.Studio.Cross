using LiteDB.Studio.Cross.Contracts.DTO;
using LiteDB.Studio.Cross.ViewModels;

namespace LiteDB.Studio.Cross {
    public static class Mapper {
        public static ConnectionParametersDto Map(this ConnectionParametersViewModel vm) {
            var result = new ConnectionParametersDto {
                DbPath = vm.DbPath, Password = vm.Password, Culture = vm.Culture, Sort = vm.Sort,
                IsShared = vm.IsShared,
                InitSize = vm.InitSize,
                IsUpgrade = vm.IsUpgrade,
                IsReadOnly = vm.IsReadOnly
            };
            return result;
        }
    }
}

