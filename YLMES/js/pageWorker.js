
    "use strict"
    function digit(t, e) {
        var i = "";
        t = String(t),
            e = e || 2;
        for (var a = t.length; a < e; a++)
            i += "0";
        return t < Math.pow(10, e) ? i + (0 | t) : t
    }
    function toDateString(t, e) {
        if (typeof (t) == "string") {
            t = Number(t.substring(t.indexOf("(") + 1, t.indexOf(")")))
        }
        var a = new Date(t || new Date)
            , n = [digit(a.getFullYear(), 4), digit(a.getMonth() + 1), digit(a.getDate())]
            , r = [digit(a.getHours()), digit(a.getMinutes()), digit(a.getSeconds())];
        return e = e || "yyyy-MM-dd HH:mm:ss",
            e.replace(/yyyy/g, n[0]).replace(/MM/g, n[1]).replace(/dd/g, n[2]).replace(/HH/g, r[0]).replace(/mm/g, r[1]).replace(/ss/g, r[2])
    }
    onmessage = function (a) {
        var search = new String(a.data.search)
        var res = a.data.vm.filter(function (data) {
            var boo = true
            var datas;
            for (var d in a.data.vm[0]) {
                if (d != "CreatedTime") {
                    if (new String(data[d]).toLowerCase().includes(search.toLowerCase())) {
                        a.data.restaurants.includes(data[d]) ? "" : a.data.restaurants.push(data[d])
                    }
                } else {
                    if (new String(toDateString(data["CreatedTime"])).toLowerCase().includes(search.toLowerCase())) {
                        a.data.restaurants.includes(toDateString(data["CreatedTime"])) ? "" : a.data.restaurants.push(toDateString(data.CreatedTime))
                    }
                }
                if (boo) {
                    if (d != "CreatedTime") {
                        if (new String(data[d]).toLowerCase().includes(search.toLowerCase())) {
                            boo = false
                            datas = data
                        }
                    } else {
                        if (new String(toDateString(data["CreatedTime"])).toLowerCase().includes(search.toLowerCase())) {
                            boo = false
                            datas = data
                        }
                    }
                }
            }
            return datas
        })
        var d = { vm: res, restaurants: a.data.restaurants }
        this.postMessage(d)
    }