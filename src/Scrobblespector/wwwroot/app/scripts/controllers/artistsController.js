(function () {
    angular
        .module('scrobblespectorApp')
        .controller('ArtistsController', ArtistsController);

    ArtistsController.$inject = ['artistsService', 'utilizr'];

    function ArtistsController(artistsService, utilizr) {
        var vm = this;
        vm.searchInProgress = false;
        vm.artistName = 'Gary Moore';
        vm.foundArtists = [];
        vm.totalArtistsFound = 0;
        vm.selectedArtist = null;
        vm.selectedArtistInfo = null;
        vm.selectedArtistDefaultImageUrl = null;
        vm.activeTab = 1;

        vm.searchArtist = function (keyEvent) {
            if (keyEvent !== null && keyEvent.which !== 13) {
                return;
            }

            if (vm.artistName.length === 0 || vm.searchInProgress) {
                return;
            }

            vm.selectedArtist = null;
            vm.selectedArtistInfo = null;
            vm.searchInProgress = true;
            artistsService.searchArtist(vm.artistName).then(function (data) {
                vm.foundArtists = data.value.foundArtists;
                vm.totalArtistsFound = data.value.totalCount;
                vm.searchInProgress = false;
            }, function (error) {
                console.error('error => ' + error)
                vm.searchInProgress = false;
            });
        };

        vm.selectArtist = function (artist) {
            if (artist.isSelected) {
                return;
            }

            vm.activeTab = 1;
            vm.diselectAllArtists();
            vm.selectedArtist = artist;

            artistsService.getArtist(vm.selectedArtist.mbid).then(function (data) {
                if (utilizr.isNullOrUndef(data)) {
                    console.error('Get Artist Info Error: received null data!');
                    return;
                }

                console.log(data);
                vm.selectedArtistInfo = data.value;
                angular.forEach(vm.selectedArtistInfo.images, function (image) {
                    if (image.imageSize === 6) {
                        vm.selectedArtistDefaultImageUrl = image.url;
                    }
                });

                ///// get artist top tags //////////////////
                if (utilizr.isNullOrUndef(vm.selectedArtistInfo.topTags)) {
                    getArtistTopTags(vm.selectedArtist.mbid, function (topTags) {
                        vm.selectedArtist.topTags = topTags.slice(0, 10);
                    });
                }
            }, function (error) {
                console.error('error [getArtist] => ' + error)
                vm.selectedArtistInfo = null;
            });

            artist.isSelected = true;
        };

        vm.setSimilarArtists = function () {
            if (utilizr.isNotNullAndUndef(vm.selectedArtist.similarArtistsList)) {
                return;
            }

            artistsService.getSimilarArtists(vm.selectedArtist.mbid).then(function (data) {
                if (utilizr.isNullOrUndef(data)) {
                    console.error('Get Similar Artist Error: received null data!');
                    return;
                }

                vm.selectedArtist.similarArtistsList = data.value.foundArtists;
            }, function (error) {
                console.error('error [getSimilarArtists] => ' + error)
                vm.selectedArtist.similarArtistsList = null;
            });
        };

        vm.setTopTracks = function () {
            if (utilizr.isNotNullAndUndef(vm.selectedArtist.topTracks)) {
                return;
            }

            artistsService.getArtistTopTracks(vm.selectedArtist.mbid).then(function (data) {
                if (utilizr.isNullOrUndef(data)) {
                    console.error('Get artist top tracks: received null data!');
                    return;
                }

                vm.selectedArtist.topTracks = data.value;
            }, function (error) {
                console.error('error [setTopTracks] => ' + error)
                vm.selectedArtist.topTracks = null;
            });
        };

        vm.diselectAllArtists = function () {
            angular.forEach(vm.foundArtists, function (artist) {
                artist.isSelected = false;
            });
        };

        vm.openTagPage = function (tagUrl) {
            window.open(tagUrl, '_blank');
        };

        function getArtistTopTags(mbid, callback) {
            artistsService.getArtistTopTags(mbid).then(function (data) {
                if (utilizr.isNullOrUndef(data)) {
                    console.error('Get Artist Info Error: received null data!');
                    return;
                }

                callback(data.value);
            }, function (error) {
                console.error('error [getArtistTopTags] => ' + error)
                vm.selectedArtistInfo.topTags = null;
                callback([]);
            });
        }
    }
})();