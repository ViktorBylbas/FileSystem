FSApp.controller('FSAppController', function TableController($scope, $http) {
    getCountFiles('');
    getCurrentPath('');
    getFiles(null);

    $scope.dirName = function (name) {
        if (name == "..") {
            path = path.substring(0, path.length - 1);
            lastSlash = path.lastIndexOf("\\");
            if (lastSlash != -1)
                path = path.substring(0, lastSlash) + "\\";
            else
                path = "";
        }
        else
            path += name + "\\";

        getCountFiles(path);
        getFiles(path);
        getCurrentPath(path);
    }

    function getCountFiles(path) {
        $http.post('/Home/GetCountFiles', { Path: path }).then(function (data) {
            $scope.count = data.data;
        });
    }

    function getFiles(path) {
        $http.post('/Home/GetFiles', { Path: path }).then(function (response) {
            $scope.dirs = response.data.dirs;
            $scope.files = response.data.files;
        });
    }

    function getCurrentPath(path) {
        $http.post('/Home/GetCurrentPath', { Path: path }).then(function (data) {
            $scope.path = data.data;
        });
    }
});