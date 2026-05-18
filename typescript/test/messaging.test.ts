import type { Message } from '../src/messaging'
import { encode, decode } from '@msgpack/msgpack'
import {
    MessageValidationError,
    createMessage,
    createAndSerializeMessage,
    serializeMessage,
    deserializeMessage
} from '../src/messaging'

import { expect, test } from 'vitest'

test('Successfully Creates a Message', () => {
    const messageType: string = "Test";
    const receivers: string[] = ["Hello", "World!"];
    const content: Record<string, any> = {
        "Hello": "World",
        "One": 1,
        "Cool?": true
    };

    const message: Message = createMessage(messageType, receivers, content);

    expect(message.type).toBe(messageType);

    for (let receiver of receivers) {
        expect(message.receivers).toContain(receiver);
    }

    for (let [key, value] of Object.entries(content)) {
        expect(message.content[key]).toBe(value);
    }
})

test('Successfully Catch Serialization Errors', () => {
    // No, despite what an LSP might tell you, these are not mistakes
    expect(() => serializeMessage(null as Message)).toThrow(/null/);
    expect(() => serializeMessage(undefined as Message)).toThrow(/undefined/);
})

test('Successfully Catch Corrupted Message During Decode', () => {
    const validMessage: Message = {
        type: "Test",
        receivers: ["Game", "Player1", "Player2"],
        content: {
            "Suspect": "Player1",
            "Suspected By": "Player2",
            "Guesses Left": 1,
        }
    };

    let corruptedMessage: Uint8Array = serializeMessage(validMessage);

    corruptedMessage = corruptedMessage.slice(0, corruptedMessage.length - 2);

    expect(() => deserializeMessage(corruptedMessage)).toThrow(/Corrupted/);
})

test('Successfully Catch Invalid Messages', () => {
    const invalidType = {
        type: 1,
        receivers: ["Doesn't Matter", "E"],
        content: {
            "Scrimblo": "Mode",
            "Nothing": "Matters"
        }
    };
    const invalidReceiversContents = {
        type: "test",
        receivers: ["False", 0],
        content: "Hello"
    };
    const invalidReceiversType = {
        type: "test",
        receivers: 0,
        content: "What?"
    };

    const missingType = {
        receivers: ["Doesn't Matter"],
        content: "Hello"
    };

    const missingReceivers = {
        type: "Error",
        content: "What?"
    };

    // Content can (in theory) be any type, so this is the only relevant test
    const missingObject = {
        type: "Error",
        receivers: ["John", "Jane", "Sarah"]
    };

    const encodedInvalidType = encode(invalidType);
    const encodedInvalidReceiverContents = encode(invalidReceiversContents);
    const encodedInvalidReceiversType = encode(invalidReceiversType);
    const encodedMissingType = encode(missingType);
    const encodedMissingReceivers = encode(missingReceivers);
    const encodedMissingObject = encode(missingObject);

    expect(() => deserializeMessage(encodedInvalidType)).toThrow(/Unable to Decode and Validate/);
    expect(() => deserializeMessage(encodedInvalidReceiverContents)).toThrow(/Unable to Decode and Validate/);
    expect(() => deserializeMessage(encodedInvalidReceiversType)).toThrow(/Unable to Decode and Validate/);
    expect(() => deserializeMessage(encodedMissingType)).toThrow(/Unable to Decode and Validate/);
    expect(() => deserializeMessage(encodedMissingReceivers)).toThrow(/Unable to Decode and Validate/);
    expect(() => deserializeMessage(encodedMissingObject)).toThrow(/Unable to Decode and Validate/);
})

test("Full Message Transfer", () => {
    const messageType: string = "Transfer Test";
    const receivers: string[] = ["Me", "Myself", "I"];
    const content: Record<string, any> = {
        "Hello": "World",
        "One": 1,
        "Mad": false
    };

    const bytes: Uint8Array = createAndSerializeMessage(messageType, receivers, content);

    const message: Message = deserializeMessage(bytes);

    expect(message.type).toBe(messageType);

    for (let receiver of receivers) {
        expect(message.receivers).toContain(receiver);
    }

    for (let [key, value] of Object.entries(content)) {
        expect(message.content[key]).toBe(value);
    }
})
