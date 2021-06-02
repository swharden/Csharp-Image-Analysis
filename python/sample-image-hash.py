from PIL import Image
import pathlib
import numpy as np
import hashlib


def validateGrayscale():

    imagePath = pathlib.Path(__file__).parent\
        .parent.joinpath("data/images/coins.png")

    print(f"\n{imagePath.resolve()}")
    with Image.open(imagePath) as im:
        data = np.asarray(im)

    print(f"Data Shape: {data.shape}")
    assert (303, 384) == data.shape

    flat = data.flatten()
    print(f"Flattened: {flat}")

    assert flat.dtype == 'uint8'
    print(f"numpy array type: {flat.dtype}")

    hash = hashlib.md5(flat).hexdigest()
    assert hash == '651a9e413b9cc780d6ae9c5eca027c76'
    print(f"HASH: {hash}")


def validateRGB():

    imagePath = pathlib.Path(__file__).parent\
        .parent.joinpath("data/images/cat.png")

    print(f"\n{imagePath.resolve()}")
    with Image.open(imagePath) as im:
        data = np.asarray(im)

    print(f"Data Shape: {data.shape}")
    assert (300, 451, 3) == data.shape

    flat = data.flatten()
    print(f"Flattened: {flat}")

    assert flat.dtype == 'uint8'
    print(f"numpy array type: {flat.dtype}")

    hash = hashlib.md5(flat).hexdigest()
    assert hash == '4cbc8458da90b6c4b2dcf19e51656619'
    print(f"HASH: {hash}")


if __name__ == "__main__":
    validateGrayscale()
    validateRGB()
