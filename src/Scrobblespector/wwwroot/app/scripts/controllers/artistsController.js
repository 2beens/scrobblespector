(function () {
    angular
        .module('scrobblespectorApp')
        .controller('ArtistsController', ArtistsController);

    ArtistsController.$inject = ['artistsService'];

    function ArtistsController(artistsService) {
        var vm = this;
        vm.searchInProgress = false;
        vm.artistName = '';
        vm.foundArtists = [];
        vm.selectedArtist = null;

        vm.searchArtist = function (keyEvent) {
            if (keyEvent !== null && keyEvent.which !== 13) {
                return;
            }

            if (vm.artistName.length === 0 || vm.searchInProgress) {
                return;
            }

            vm.selectedArtist = null;
            vm.searchInProgress = true;
            artistsService.searchArtist(vm.artistName).then(function (data) {
                vm.foundArtists = data.value.artistmatches.artist;
                vm.searchInProgress = false;
            }, function (error) {
                alert('error => ' + error)
                vm.searchInProgress = false;
            });
        }

        vm.selectArtist = function (artist) {
            if (artist.isSelected) {
                return;
            }

            vm.diselectAllArtists();
            vm.selectedArtist = artist;

            artistsService.getArtist(vm.selectedArtist.mbid).then(function (data) {
                console.log(data);
            }, function (error) {
                alert('error => ' + error)
            });

            artist.isSelected = true;
        }

        vm.diselectAllArtists = function () {
            angular.forEach(vm.foundArtists, function (artist) {
                artist.isSelected = false;
            });
        }
    }
})();