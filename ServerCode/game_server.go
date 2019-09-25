package main

import (
	"bufio"
	"fmt"
	"net"
	"strconv"
)

func main() {
	fmt.Println("Server Started ..... ")
	terminateConnection := false
	started := false
	for terminateConnection == false {
		if started == false {
			go listenForConnections(&terminateConnection)
			started = true
		}
	}
}
func listenForConnections(terminateConnection *bool) {
	playersConnected := 0
	var connections [2]net.Conn
	var buffer [2]string
	buffer[0] = ""
	buffer[1] = ""
	listener, error := net.Listen("tcp", "localhost:3000")
	for playersConnected < 2 {
		if error != nil {
			*terminateConnection = true
			return
		}
		connections[playersConnected], error = listener.Accept()
		if error != nil {
			*terminateConnection = true
			return
		}
		fmt.Println("Connection Initiated " + strconv.Itoa(playersConnected+1))
		fmt.Println(connections[playersConnected])
		connections[playersConnected].Write([]byte(strconv.Itoa(playersConnected)))
		playersConnected++

	}
	//start Game on both ends
	//connections[0].Write([]byte("start"))
	//connections[1].Write([]byte("start"))
	// Process Connections
	go processConnections(connections, buffer, 0, terminateConnection)
	go processConnections(connections, buffer, 1, terminateConnection)
	//go processConnections(connections[1], &buffer[1], 1, terminateConnection)
	//Run Server Infinitely
	//for {
	//	connections[0].Write([]byte(buffer[0]))
	//	connections[1].Write([]byte(buffer[0]))
	//}
}
func processConnections(connections [2]net.Conn, buffer [2]string, index int, terminateConnection *bool) {
	fmt.Println("inside process Connection ")
	bufioReader := bufio.NewReader(connections[index])
	for {
		bytes, error := bufioReader.ReadBytes('\n')
		if error != nil {
			*terminateConnection = true
			fmt.Println(error)
			return
		}
		//fmt.Print(string(bytes))
		bufferData := string(bytes)
		bufferData = bufferData[:len(bufferData)-1]
		buffer[index] = bufferData
		fmt.Println(buffer[index])
		connections[(index+1)%2].Write([]byte(bufferData))
	}
}
