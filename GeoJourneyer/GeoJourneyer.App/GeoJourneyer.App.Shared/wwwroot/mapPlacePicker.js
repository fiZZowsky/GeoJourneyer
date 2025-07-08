window.mapPlacePicker = {
    init: function (id, places, selectedIds, dotnetHelper) {
        if (!window.google || !places) return;
        const mapEl = document.getElementById(id);
        if (!mapEl || places.length === 0) return;
        const center = { lat: places[0].latitude, lng: places[0].longitude };
        const map = new google.maps.Map(mapEl, { zoom: 6, center: center });

        const selected = new Set(selectedIds || []);
        const infoWindow = new google.maps.InfoWindow();
        const normalIcon = 'http://maps.google.com/mapfiles/ms/icons/red-dot.png';
        const selectedIcon = 'http://maps.google.com/mapfiles/ms/icons/green-dot.png';

        places.forEach(p => {
            const marker = new google.maps.Marker({
                position: { lat: p.latitude, lng: p.longitude },
                map: map,
                title: p.name,
                icon: selected.has(p.id) ? selectedIcon : normalIcon
            });
            marker.addListener('click', () => {
                const isSel = selected.has(p.id);
                if (isSel) {
                    selected.delete(p.id);
                    marker.setIcon(normalIcon);
                } else {
                    selected.add(p.id);
                    marker.setIcon(selectedIcon);
                }
                if (dotnetHelper) {
                    dotnetHelper.invokeMethodAsync('TogglePlace', p.id, !isSel);
                }
                const content = `<div><strong>${p.name}</strong><br/><a href="https://www.google.com/maps/search/?api=1&query=${p.latitude},${p.longitude}" target="_blank">View details</a></div>`;
                infoWindow.setContent(content);
                infoWindow.open(map, marker);
            });
        });
    }
};
