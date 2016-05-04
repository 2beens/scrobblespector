﻿(function () {
    angular
        .module('scrobblespectorApp')
        .controller('ArtistsController', ArtistsController);

    ArtistsController.$inject = ['artistsService'];

    function ArtistsController(artistsService) {
        var vm = this;
        vm.searchInProgress = false;
        vm.artistName = '';
        vm.foundArtists = [];

        vm.searchArtist = function (keyEvent) {
            if (keyEvent !== null && keyEvent.which !== 13) {
                return;
            }

            if (vm.artistName.length === 0 || vm.searchInProgress) {
                return;
            }

            vm.searchInProgress = true;
            artistsService.searchArtist(vm.artistName).then(function (data) {
                vm.foundArtists = data.artistmatches.artist;
                vm.searchInProgress = false;
            }, function (error) {
                alert('error => ' + error)
                vm.searchInProgress = false;
            });
        }
    }
})();