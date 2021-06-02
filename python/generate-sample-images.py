"""
Use a common library (skimage) as a source of truth for
experimental images to analyze
"""

import skimage
import pathlib
import numpy as np
import matplotlib.pyplot as plt
from PIL import Image


def saveImage(data: np.ndarray, name: str):

    # save it to disk
    repoRoot = pathlib.Path(__file__).parent.parent
    dataFolder = repoRoot.joinpath("data/images")
    if len(data.shape) == 2:
        np.savetxt(dataFolder.joinpath(f"{name}.txt"), data, fmt="%d")
    Image.fromarray(data).save(dataFolder.joinpath(f"{name}.png"))

    # read it from disk and verify it matches
    data2 = np.array(Image.open(dataFolder.joinpath(f"{name}.png")))
    assert(data2.all() == data.all())


if __name__ == "__main__":
    imageFuncs = [
        "astronaut", "binary_blobs", "brick", "camera", "cat", "cell",
        "checkerboard", "chelsea", "clock", "coffee", "coins",
        "colorwheel", "grass", "gravel", "horse", "hubble_deep_field",
        "immunohistochemistry", "logo", "microaneurysms",
        "moon", "page", "retina", "rocket", "stereo_motorcycle",
    ]
    for funcName in imageFuncs:
        print(funcName)
        func = getattr(skimage.data, funcName)
        data = func()
        saveImage(data, funcName)
