var tag = false;

function confirmO(id, msg) {
    if (tag) return true;
    Boxy.confirm(msg, function() { tag = true; $(id).click(); }, null);
    return tag;
}