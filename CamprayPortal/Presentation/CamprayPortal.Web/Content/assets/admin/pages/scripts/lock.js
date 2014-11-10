var Lock = function () {

    return {
        //main function to initiate the module
        init: function () {

             $.backstretch([
		        "/Content/assets/admin/pages/media/bg/1.jpg",
    		    "/Content/assets/admin/pages/media/bg/2.jpg",
    		    "/Content/assets/admin/pages/media/bg/3.jpg",
    		    "/Content/assets/admin/pages/media/bg/4.jpg"
		        ], {
		          fade: 1000,
		          duration: 8000
		      });
        }

    };

}();