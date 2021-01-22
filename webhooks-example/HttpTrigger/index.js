const crypto = require('crypto');

module.exports = async function (context, req) {



    // Check the secret first: 
    const hmac = Crypto.createHmac("sha1", "<default key>");
    const signature = hmac.update(JSON.stringify(req.body)).digest('hex'); // Create HMAC SHA1 hex digest of FA default key
    const shaSignature = `sha1=${signature}`;
    const gitHubSignature = req.headers['x-hub-signature'];


    try 
    {
        if(shaSignature.localeCompare(gitHubSignature))
        {
            if (req.body.pages[0].title){
                context.res = {
                    body: "Page is " + req.body.pages[0].title + ", Action is " + req.body.pages[0].action + ", Event Type is " + req.headers['x-github-event']
                };
            }
            else {
                context.res = {
                    status: 400,
                    body: ("Invalid payload for Wiki event")
                };
            }
        }
        else 
        {
            context.res = {
                status: 401,
                body: "Signatures don't match"
            };
        }

    }
    catch(error)
    {
        context.res = {status:500, body:error } 
    }


}