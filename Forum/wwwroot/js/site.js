// Write your JavaScript code.
var enableNotificationsButton = document.querySelectorAll('.enable-notification')

function displayConfirmNotification() {
    if ('serviceWorker' in navigator) {
        var options = {
            body: 'Udało ci się zasubskrybować tę stronę!',
            icon: '/images/manifest/wz-96x96.png',
            lang: 'pl-PL',
            vibrate: [100, 50, 200],
            badge: '/images/manifest/wz-96x96.png'
        };
    }
    navigator.serviceWorker.ready
        .then(function(swreg) {
            swreg.showNotification('SW', options);
        });
}

function urlBase64ToUint8Array(base64String) {
    var padding = '='.repeat((4 - base64String.length % 4) % 4);
    var base64 = (base64String + padding)
        .replace(/\-/g, '+')
        .replace(/_/g, '/');

    var rawData = window.atob(base64);
    var outputArray = new Uint8Array(rawData.length);

    for (var i = 0; i < rawData.length; ++i) {
        outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
}

function configurePushSub() {
    if (!('serviceworker' in navigator)) {
        return;
    }
    var reg;
    navigator.serviceWorker.ready
        .then(function (swreg) {
            reg = swreg;
            return swreg.pushManager.getSubscription();
        })
        .then(function(sub) {
            if (sub === null) {
                var vapidPublicKey =
                    'BO85JUld3BJkoA897qIhGVNVsZVzbXAWSwi28Wwb2nOkTf1VDty2Sr_nfPK71e4Y2s2dYn4yEfWJlzT8PVoix9E';
                var convertedVapidPublicKey = urlBase64ToUint8Array(vapidPublicKey);
                reg.pushManager.subscribe({
                    userVisibleOnly: true,
                    applicationServerKey: convertedVapidPublicKey
                });
            } else {

            }
        })
        .then(function (newSub)
        {
    
            return fetch('https:// ....firebase...', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify(newSub)
            })
                .then(function(res) {
                    if (res.ok) {
                        displayConfirmNotification();
                    }
                })
        });
}

function askForNotificationPermission() {
    Notification.requestPermission(function(result) {
        console.log('User Choice', result);
        if (result !== 'granted') {
            console.log('No notification permission granted');
        } else {
            //displayConfirmNotification()
            configurePushSub();
        }
    })
}

if ('Notification' in window && 'serviceWorker' in navigator) {
    enableNotificationsButton[0].style.display = 'inline-block';
    enableNotificationsButton[0].addEventListener('click', askForNotificationPermission);
    pushNotificationStatus.isSupported = true;
    pushNotificationStatus.isSupported = false;
    checkSubscription()
}

function checkSubscription(){
    this.swRegistration.pushManager.getSubscription()
        .then(subscription => {
            console.log(subscription);
            console.log(JSON.stringify(subscription));
            this.pushNotificationStatus.isSubscribed = !(subscription === null);
        });
}


///////
pushNotificationStatus = {
    isSubscribed: false,
    isSupported: false
};