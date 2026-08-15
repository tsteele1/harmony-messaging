import { encode, decode } from "@msgpack/msgpack"

/*
 * A Harmony compliant Message in TypeScript.
 * Used for easy conversions to MessagePack and transfers over websockets.
 *
 * type (string):                 A user defined message type for understanding what 
 *                                data was transferred.
 *
 * receivers (string[]):          A list of receivers Harmony will send the Message to.
 *
 * content (Record<string, any>): A map containing the user's custom message data
 *                                they want sent across.
 */
export interface Message {
    type: string,
    receivers: string[],
    content: unknown
}

/*
 * Any error that might occur when attempting to desreialize a Message
 * from a Uint8Array (MessagePack).
 */
export class MessageValidationError extends Error {
    constructor(message: string, options?: ErrorOptions) {
        super(message, options);

        this.name = "MessageValdationError";

        Object.setPrototypeOf(this, MessageValidationError.prototype);
    }

}

/*
 * Alias for Creating an object of the Message Class.
 */
export function createMessage(type: string, receivers: string[], content: unknown): Message {
    return { type, receivers, content };
}

/*
 * Serialize a Message to MessagePack
 * Intended to be used to send byte data over a websocket.
 *
 * Throws a TypeError in the rare case a message is undefined or null
 * for safety.
 *
 */
export function serializeMessage(message: Message): Uint8Array {
    if (!message) {
        throw new TypeError("Cannot Serialize null or undefined Messages");
    }

    return encode([message.type, message.receivers, message.content]);
}

/*
 * A combination of createMessage and SerializeMessage
 */
export function createAndSerializeMessage(type: string, receivers: string[], content: unknown): Uint8Array {
    return encode([type, receivers, content]);
}


function isValidEncodedArray(decoded: unknown): decoded is unknown[] {
    if (!Array.isArray(decoded)) {
        return false;
    }
    else if (decoded.length != 3) {
        return false;
    }

    return true;
}

function isValidString(value: unknown): value is string {
    return typeof value == "string";
}

function isValidStringArr(value: unknown): value is string[] {
    return Array.isArray(value) && value.every(r => typeof r == "string");
}

/*
 * Convert an encodedMessage back into a regular Message.
 * Intended to be used after a Message has been received from a websocket.
 *
 * Throws a MessageValidationError if the payload cannot be decoded,
 * or if the decoded object is not a valid Message (with appropriate error
 * messages for each respectively).
 */
export function deserializeMessage(encodedMessage: Uint8Array): Message {
    let decoded: unknown;

    try {
        decoded = decode(encodedMessage);
    }
    catch (error) {
        throw new MessageValidationError("Failed to Decode Payload. Data May be Corrupted.",
            { cause: error });
    }

    if (!isValidEncodedArray(decoded)) {
        throw new MessageValidationError(`Unable to Decoded Encoded Message into a Valid Array.`);
    }
    else if (!isValidString(decoded[0])) {
        throw new MessageValidationError(`Unable to Decoded Encoded Message Type to String.`);
    }
    else if (!isValidStringArr(decoded[1])) {
        throw new MessageValidationError(`Unable to Decode Encoded Receivers to String Array.`);
    }

    const decodedMessage: Message = {
        type: decoded[0],
        receivers: decoded[1],
        content: decoded[2]
    };

    return decodedMessage;
}
