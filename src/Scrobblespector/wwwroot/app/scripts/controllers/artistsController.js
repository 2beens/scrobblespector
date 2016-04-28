(function () {
    angular
        .module('scrobblespectorApp')
        .controller('ArtistsController', ArtistsController);

    ArtistsController.$inject = [];

    function ArtistsController() {
        var vm = this;
        vm.artistName = '';

        vm.getArtist = function (artistName) {
            if (artistName.length === 0) {
                return;
            }

            alert('artist = ' + artistName);
        }
    }
})();