const { createServer } = require("http");
const { Server } = require("socket.io");

const httpServer = createServer();
const io = new Server(httpServer, {
    // options
    cors: { origin: '*', },
});
const port = 3000;

let currentCount = 0;
let countChangedBy = "";

io.on("connection", (socket) => {
    console.log("socket connection", socket.id);
    // send a welcome message
    socket.emit("welcome", `Welcome to my server! ${socket.id}`);
    // listen for disconnect
    socket.on("disconnect", () => {
        console.log("socket disconnect", socket.id);
    });
    socket.on("incrementCount", () => {
        // request to increase count
        // increase and notify all sockets
        currentCount++;
        countChangedBy = socket.id;
        // notify all sockets
        io.emit("countChanged", countChangedBy, currentCount);
    });
    socket.on("getCount", (callback) => {
        // request for current count
        callback([countChangedBy, currentCount]);
    });
    // listen for events messages
    socket.on("getWeather", function (callback) {
        console.log("socket getWeather", socket.id);
        var weather = [
            {
                "date": "2022-01-06",
                "temperatureC": 1,
                "summary": "Freezing"
            },
            {
                "date": "2022-01-07",
                "temperatureC": 14,
                "summary": "Bracing"
            },
            {
                "date": "2022-01-08",
                "temperatureC": -13,
                "summary": "Freezing"
            },
            {
                "date": "2022-01-09",
                "temperatureC": -16,
                "summary": "Balmy"
            },
            {
                "date": "2022-01-10",
                "temperatureC": -2,
                "summary": "Chilly"
            }
        ];
        callback(weather);
    });
    socket.on("testTupleReturn", (callback) => {
        console.log("socket testTupleReturn", socket.id);
        // returning tuple can be useful for returning an (error?, result?) pair
        callback([null, true]);
    });
});

// listen
httpServer.listen(port, function () {
    console.log(`Listening on port ${port}`);
});